using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Models;
using Messenger.Conversations.Common.Models.RealtimeUpdates;
using Messenger.Conversations.PrivateMessages.Models;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.RealTime.Common.Services;

namespace Messenger.Conversations.PrivateMessages.MessageActions.PrivateSendMessage;

public class PrivateSendMessageActionHandler : IMessageActionHandler<SendMessageAction, SendMessageActionResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IUpdateConnectionManager _updateConnectionManager;
    private readonly IUserService _userService;
    private readonly IDateTimeProvider _dateTimeProvider;
    public static string MessageType => ConversationTypes.PersonalChat;

    public PrivateSendMessageActionHandler(
        IDbContext dbContext,
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IUpdateConnectionManager updateConnectionManager,
        IUserService userService,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _reserveNumberHandler = reserveNumberHandler;
        _updateConnectionManager = updateConnectionManager;
        _userService = userService;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<SendMessageActionResponse> Handle(SendMessageAction request, CancellationToken cancellationToken)
    {
        var messagePosition = 
            await _reserveNumberHandler.Handle(new(request.Conversation.Id), cancellationToken);

        var toNotify = _dbContext.PersonalChatMembers
            .Where(x => x.ConversationId == request.Conversation.Id)
            .Select(x => x.UserId)
            .ToArray();

        var messageData = request.MessageData;

        var conversationMessage = new ConversationMessage()
        {
            Attachments = messageData.Attachments,
            SentAt = _dateTimeProvider.NowUtc,
            ConversationId = request.Conversation.Id,
            TextContent = messageData.TextContent, 
            SenderId = _userService.GetUserIdOrThrow(),
            Position = messagePosition,
        };

        _dbContext.ConversationMessages.Add(conversationMessage);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        _updateConnectionManager.SendToUsers(
            toNotify,
            new NewMessageRealtimeUpdate(
                request.Conversation.Id,
                new PrivateMessageListProjection(
                    conversationMessage.Id,
                    conversationMessage.SenderId.Value,
                    conversationMessage.TextContent ?? "",
                    conversationMessage.Attachments,
                    conversationMessage.SentAt,
                    conversationMessage.EditedAt,
                    conversationMessage.Position)));
 
        return new(true, conversationMessage.Id);
    }
}
