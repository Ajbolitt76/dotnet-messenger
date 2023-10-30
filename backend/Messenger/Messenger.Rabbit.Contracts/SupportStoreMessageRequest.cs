namespace Messenger.Rabbit.Contracts;

public record SupportStoreMessageRequest(Guid UserId, Guid ConversationId, string Content, uint MessagePosition);
