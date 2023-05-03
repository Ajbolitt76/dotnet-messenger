using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatMemberInfo;

public record GetGroupChatMemberInfoQuery(
    Guid CurrentUserId,
    Guid UserId,
    Guid ConversationId) : IQuery<GetGroupChatMemberInfoResponse>;
