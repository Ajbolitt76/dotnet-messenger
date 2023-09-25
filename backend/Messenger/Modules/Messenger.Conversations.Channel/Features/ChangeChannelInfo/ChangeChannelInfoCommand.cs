using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.ChangeChannelInfo;

public record ChangeChannelInfoCommand(
    Guid UserId,
    Guid ConversationId,
    string NewTitle,
    string NewDescription) : ICommand<bool>;
