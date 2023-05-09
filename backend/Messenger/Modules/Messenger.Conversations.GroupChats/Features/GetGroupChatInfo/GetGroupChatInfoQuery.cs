using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;

public record GetGroupChatInfoQuery(Guid UserId, Guid ConversationId) : IQuery<GetGroupChatInfoQueryResponse>;