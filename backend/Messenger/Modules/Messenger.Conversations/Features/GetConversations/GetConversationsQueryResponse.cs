using Messenger.Core.Model.ConversationAggregate;

namespace Messenger.Conversations.Features.GetConversations;

public record GetConversationsQueryResponse(IReadOnlyList<GetConversationsQueryResponseItem> Items);

public record ConversationLastMessage(string? AuthorName, string? Text, DateTime? SentAt);

public record GetConversationsQueryResponseItem(
    Guid Id,
    string Title,
    ConversationLastMessage LastMessage,
    string Type);
