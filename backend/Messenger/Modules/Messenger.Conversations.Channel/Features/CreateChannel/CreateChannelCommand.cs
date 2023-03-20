using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;

namespace Messenger.Conversations.Channel.Features.CreateChannel;

public record CreateChannelCommand(
    Guid InitiatorId,
    string Name) : ICommand<CreatedResponse<Guid>>;