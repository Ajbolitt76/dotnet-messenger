using Messenger.Core.Model.ConversationAggregate;

namespace Messenger.Rabbit.Contracts;

public record SupportSendMessageRequest(Guid ConversationId, ConversationMessage Message);
