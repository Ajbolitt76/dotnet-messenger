using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Channel.Features.GiveChannelPermissions;

public record GiveChannelPermissionsCommand(
    Guid FromUserId,
    Guid ToUserId,
    Guid ConversationId,
    IEnumerable<ChannelMemberPermissions> Permissions,
    bool MakeAdmin) : ICommand<bool>;
