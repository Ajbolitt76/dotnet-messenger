using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Features.GetConversations;

public record GetConversationsQuery() : IQuery<GetConversationsQueryResponse>;