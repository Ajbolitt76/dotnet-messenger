using Messenger.Core.Model.ConversationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.Common.Extensions;

public static class ConversationQueries
{
    public static async Task<uint> GetLatestMessagePositionAsync(this IQueryable<ConversationMessage> conversations, Guid conversationId)
    {
        return await conversations
            .Where(x => x.ConversationId == conversationId)
            .MaxAsync(x => (uint?)x.Position) ?? 0;
    }

    public static string GetConversationLock(Guid id) => $"conversation:{id}:lock";
}
