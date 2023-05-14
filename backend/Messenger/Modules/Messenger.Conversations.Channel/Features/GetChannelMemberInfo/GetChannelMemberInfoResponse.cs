using Messenger.Core.Model.ConversationAggregate.Permissions;

namespace Messenger.Conversations.Channel.Features.GetChannelMemberInfo;

public record GetChannelMemberInfoResponse(
    Guid UserId,
    Guid ConversationId,
    ChannelMemberPermissions Permissions,
    bool IsAdmin,
    bool IsOwner);
