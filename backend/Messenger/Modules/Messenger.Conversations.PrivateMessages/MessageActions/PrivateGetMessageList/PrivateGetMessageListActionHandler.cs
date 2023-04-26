using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Extensions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.PrivateMessages.Models;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.PrivateMessages.MessageActions.PrivateGetMessageList;

public class PrivateGetMessageListActionHandler
    : IMessageActionHandler<GetMessageListAction, GetMessageListActionResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    public static string MessageType => ConversationTypes.PersonalChat;

    public PrivateGetMessageListActionHandler(
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
        var messages = await _dbContext.ConversationMessages
            .GetStartingFromPointer(_dbContext, request, _userService.GetUserIdOrThrow())
            .Select(
                x => new PrivateMessageListProjection(
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
