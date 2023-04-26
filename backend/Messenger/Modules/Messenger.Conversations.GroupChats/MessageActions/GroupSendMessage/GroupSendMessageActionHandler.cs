using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.MessageActions.GroupSendMessage;

public class GroupSendMessageActionHandler : IMessageActionHandler<SendMessageAction, SendMessageActionResponse>
{
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserService _userService;
    public static string MessageType => ConversationTypes.GroupChat;

    public GroupSendMessageActionHandler(
        IDbContext dbContext,
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IUserService userService,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _reserveNumberHandler = reserveNumberHandler;
        _userService = userService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<SendMessageActionResponse> Handle(SendMessageAction request, CancellationToken cancellationToken)
    {
        var messagePosition =
            await _reserveNumberHandler.Handle(new(request.Conversation.Id), cancellationToken);

        var currentUserId = _userService.GetUserIdOrThrow();

        var member =
            await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(currentUserId, request.Conversation.Id);
        member
            .CheckForBanOrExcludeAndThrow()
            .CheckForMuteAndThrow(_dateTimeProvider.NowUtc)
            .CheckForPermissionsAndThrow(GroupMemberPermissions.SendMessages);

        var messageData = request.MessageData;

        var conversationMessage = new ConversationMessage()
        {
            Attachments = messageData.Attachments,
            SentAt = _dateTimeProvider.NowUtc,
            ConversationId = request.Conversation.Id,
            TextContent = messageData.TextContent,
            SenderId = currentUserId,
            Position = messagePosition,
        };

        _dbContext.ConversationMessages.Add(conversationMessage);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return new(true, conversationMessage.Id);
    }
}
