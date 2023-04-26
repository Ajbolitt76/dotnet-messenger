using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Extensions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Exceptions;
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

    public async Task<GetMessageListActionResponse> Handle(
        GetMessageListAction request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _userService.GetUserIdOrThrow();
        
        var isAllowed = _dbContext.GroupChatMembers.Any(
            member => member.UserId == currentUserId && 
                      member.ConversationId == request.ConversationId && 
                      !member.WasBanned && 
                      !member.WasExcluded);
        
        if (!isAllowed)
            throw new ForbiddenException();

        var messages = await _dbContext.ConversationMessages
            .GetStartingFromPointer(_dbContext, request, currentUserId)
            .Select(
                x => new GroupMessageListProjection(
                    x.Id,
                    x.SenderId.GetValueOrDefault(),
                    x.TextContent!,
                    x.Attachments,
                    x.SentAt,
                    x.EditedAt,
                    x.Position))
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetMessageListActionResponse(messages);
    }
}
