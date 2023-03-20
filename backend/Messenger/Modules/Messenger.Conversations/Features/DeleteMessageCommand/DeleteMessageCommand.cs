using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Features.DeleteMessageCommand;

public record DeleteMessageCommand(Guid ConversationId, Guid MessageId, bool DeleteFromAll) : ICommand<bool>;
