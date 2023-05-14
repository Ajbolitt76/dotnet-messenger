using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.JoinChannel;

public record JoinChannelCommand(Guid CurrentUserId, Guid ConversationId) : ICommand<bool>;
