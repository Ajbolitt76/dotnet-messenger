using Messenger.Conversations.Common.Models.RealtimeUpdates;
using Messenger.Conversations.PrivateMessages.Models;
using Messenger.Core.Requests.Abstractions;
using Messenger.Rabbit.Contracts;
using Messenger.RealTime.Common.Services;

namespace Messenger.Support.MessageActions.SupportSendMessage;

public class SupportSendMessageActionHandler : ISupportSendMessageActionHandler
{
    private readonly IDbContext _dbContext;
    private readonly IUpdateConnectionManager _updateConnectionManager;

    public SupportSendMessageActionHandler(
        IDbContext dbContext,
        IUpdateConnectionManager updateConnectionManager)
    {
        _dbContext = dbContext;
        _updateConnectionManager = updateConnectionManager;
    }
    
    public async Task<bool> Handle(SupportSendMessageRequest request, CancellationToken cancellationToken)
    {
        var toNotify = _dbContext.PersonalChatMembers
            .Where(x => x.ConversationId == request.ConversationId)
            .Select(x => x.UserId)
            .ToArray();
        
        var conversationMessage = request.Message;
        
        _updateConnectionManager.SendToUsers(
            toNotify,
            new NewMessageRealtimeUpdate(
                request.ConversationId,
                new PrivateMessageListProjection(
                    conversationMessage.Id,
                    conversationMessage.SenderId!.Value,
                    conversationMessage.TextContent ?? "",
                    conversationMessage.Attachments,
                    conversationMessage.SentAt,
                    conversationMessage.EditedAt,
                    conversationMessage.Position)));

        return true;
    }
}
