namespace Messenger.Support.Models;

public record SupportMessage(Guid UserId, Guid ConversationId, string Message);
