using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.MuteGroupMember;

public record MuteGroupMemberCommand(
    Guid FromUserId,
    Guid ToUserId,
    Guid ConversationId,
    TimeSpan TimeSpan) : ICommand<bool>;
