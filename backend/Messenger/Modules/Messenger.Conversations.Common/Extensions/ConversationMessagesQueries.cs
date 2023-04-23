using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Common.Extensions;

public static class ConversationMessagesQueries
{
    public static IQueryable<ConversationMessage> GetStartingFromPointer(
        this IQueryable<ConversationMessage> cm,
        IDbContext dbContext,
        GetMessageListAction action,
        Guid userId)
    {
        return cm.Where(
                x =>
                    x.ConversationId == action.ConversationId
                    && (x.DeletedFrom == null || !x.DeletedFrom.Contains(userId))
                    && (action.MessagePointer == null
                        || (action.Count > 0
                            ? x.Position < dbContext.ConversationMessages
                                .FirstOrDefault(y => y.Id == action.MessagePointer)
                                !.Position
                            : x.Position > dbContext.ConversationMessages
                                .FirstOrDefault(y => y.Id == action.MessagePointer)
                                !.Position)))
            .OrderByDescending(x => x.Position)
            .Take(Math.Abs(action.Count));
    }
}
