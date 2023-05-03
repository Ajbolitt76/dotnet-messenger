using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.UnbanGroupMember;

public record UnbanGroupMemberCommand(Guid FromUserId, Guid ToUserId, Guid ConversationId) : ICommand<bool>;