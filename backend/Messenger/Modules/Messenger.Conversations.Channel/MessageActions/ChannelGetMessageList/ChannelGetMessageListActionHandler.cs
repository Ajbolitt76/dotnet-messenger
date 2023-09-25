using Messenger.Conversations.Channel.Models;
using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Extensions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.Channel.MessageActions.ChannelGetMessageList;

public class ChannelGetMessageListActionHandler : IMessageActionHandler<GetMessageListAction, GetMessageListActionResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;

    public static string MessageType => ConversationTypes.Channel;
    
    public ChannelGetMessageListActionHandler(IDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<GetMessageListActionResponse> Handle(GetMessageListAction request, CancellationToken cancellationToken)
    {
        var currentUserId = _userService.GetUserIdOrThrow();

        var messages = await _dbContext.ConversationMessages
            .GetStartingFromPointer(_dbContext, request, currentUserId)
            .Select(
                x => new ChannelMessageListProjection(
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
