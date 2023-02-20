using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;

public record CreatePrivateConversationCommand(
    Guid InitiatorId,
    Guid ReceiverId
) : ICommand<Guid>;
