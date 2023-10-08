using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.Models.RealtimeUpdates;
using Messenger.Conversations.PrivateMessages.Models;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.RealTime.Common.Services;
using Messenger.Support.Models;

namespace Messenger.Support.MessageActions.SupportSendMessage;

public class SupportSendMessageActionHandler : ISupportSendMessageActionHandler
{
    private readonly IDbContext _dbContext;
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IUpdateConnectionManager _updateConnectionManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    public static string MessageType => ConversationTypes.PersonalChat;

    public SupportSendMessageActionHandler(
        IDbContext dbContext,
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IUpdateConnectionManager updateConnectionManager,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _reserveNumberHandler = reserveNumberHandler;
        _updateConnectionManager = updateConnectionManager;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<bool> Handle(SupportMessage request, CancellationToken cancellationToken)
    {
        var messagePosition = 
            await _reserveNumberHandler.Handle(new(request.ConversationId), cancellationToken);

        var toNotify = _dbContext.PersonalChatMembers
            .Where(x => x.ConversationId == request.ConversationId)
            .Select(x => x.UserId)
            .ToArray();

        var conversationMessage = new ConversationMessage()
        {
            Attachments = null,
            SentAt = _dateTimeProvider.NowUtc,
            ConversationId = request.ConversationId,
            TextContent = request.Message, 
            SenderId = request.UserId,
            Position = messagePosition,
        };

        _dbContext.ConversationMessages.Add(conversationMessage);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        _updateConnectionManager.SendToUsers(
            toNotify,
            new NewMessageRealtimeUpdate(
                request.ConversationId,
                new PrivateMessageListProjection(
                    conversationMessage.Id,
                    conversationMessage.SenderId.Value,
                    conversationMessage.TextContent ?? "",
                    conversationMessage.Attachments,
                    conversationMessage.SentAt,
                    conversationMessage.EditedAt,
                    conversationMessage.Position)));

        return true;
    }
}
