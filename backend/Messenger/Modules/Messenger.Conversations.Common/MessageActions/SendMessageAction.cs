using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Models;
using Messenger.Core.Model.ConversationAggregate;

namespace Messenger.Conversations.Common.MessageActions;

public record SendMessageAction(Conversation Conversation, MessageData MessageData) : IMessageAction<bool>;
    