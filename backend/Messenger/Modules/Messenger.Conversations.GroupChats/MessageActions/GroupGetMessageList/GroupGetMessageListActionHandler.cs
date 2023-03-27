using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.MessageActions.GroupGetMessageList;

public class GroupGetMessageListActionHandler
    : IMessageActionHandler<GetMessageListAction, GetMessageListActionResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    public static string MessageType => ConversationTypes.GroupChat;

    public GroupGetMessageListActionHandler(
        IDbContext dbContext,
        IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }
    
    public async Task<GetMessageListActionResponse> Handle(GetMessageListAction request, CancellationToken cancellationToken)
    {
        //TODO: Base class
        
        //TODO: Валидация
        //1. Участник чата
        //2. Не в бане

        var messages = await _dbContext.ConversationMessages
            .Where(
                x =>
                    x.ConversationId == request.ConversationId
                    && !x.DeletedFrom!.Contains(_userService.UserId!.Value)
                    && (request.MessagePointer == null
                        || x.Position < _dbContext.ConversationMessages
                            .FirstOrDefault(x => x.Id == request.MessagePointer)
                            .Position))
            .OrderByDescending(x => x.Position)
            .Take(request.Count)
            .Select(
                x => new GroupMessageListProjection(
                    x.Id,
                    x.TextContent!,
                    x.Attachments,
                    x.SentAt,
                    x.EditedAt,
                    x.Position))
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetMessageListActionResponse(messages);
    }
}
