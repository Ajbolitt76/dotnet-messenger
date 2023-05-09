using Messenger.Core.Model.ConversationAggregate.Permissions;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatMemberInfo;

public record GetGroupChatMemberInfoResponse(
    Guid UserId,
    Guid ConversationId,
    bool WasExcluded,
    bool WasBanned,
    DateTime MutedTill,
    GroupMemberPermissions Permissions,
    bool IsAdmin,
    bool IsOwner);
