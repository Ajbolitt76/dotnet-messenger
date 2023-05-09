using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.GivePermissions;

public record GivePermissionsCommand(
    Guid FromUserId,
    Guid ToUserId,
    Guid ConversationId,
    IEnumerable<GroupMemberPermissions> Permissions,
    bool MakeAdmin) : ICommand<bool>;
