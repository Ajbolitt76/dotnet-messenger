using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.BanOrKickGroupMember;

public record BanOrKickGroupMemberCommand(
    PenaltyScopes Penalty,
    Guid FromUserId,
    Guid ToUserId,
    Guid ConversationId) : ICommand<bool>;
