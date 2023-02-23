using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;

namespace Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;

public record CreatePrivateConversationCommand(
    Guid InitiatorId,
    Guid ReceiverId
) : ICommand<CreatedResponse<Guid>>;
