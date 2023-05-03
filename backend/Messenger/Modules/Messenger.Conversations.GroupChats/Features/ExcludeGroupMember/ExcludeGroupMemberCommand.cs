using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.ExcludeGroupMember;

public record ExcludeGroupMemberCommand(
    bool Ban,
    Guid FromUserId,
    Guid ToUserId,
    Guid ConversationId) : ICommand<bool>;
