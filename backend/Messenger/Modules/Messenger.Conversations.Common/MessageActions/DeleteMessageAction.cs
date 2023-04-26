using Messenger.Conversations.Common.Abstractions;
using Messenger.Core.Model.ConversationAggregate;

namespace Messenger.Conversations.Common.MessageActions;

public record DeleteMessageAction(
    Guid ConversationId,
    Guid MessageId,
    bool DeleteFromAll,
    Conversation Conversation) : IMessageAction<bool>;
