using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.LeaveChannel;

public record LeaveChannelCommand(Guid CurrentUserId, Guid ConversationId) : ICommand<bool>;