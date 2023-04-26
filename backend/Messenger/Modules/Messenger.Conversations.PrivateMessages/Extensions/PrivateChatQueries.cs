using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Requests.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.PrivateMessages.Extensions;

public static class PrivateChatQueries
{
    /// <summary>
    /// Достать переписку с между пользователями
    /// Игнорирует инициатора
    /// </summary>
    /// <param name="queryable">Контекст БД</param>
    /// <param name="firstUser">ID первого пользователя</param>
    /// <param name="secondUser">ID второго пользователя</param>
    /// <returns>Инфа о переписке или null</returns>
    public static Task<PersonalChatInfo?> GetConversationBetweenPeersAsync(
        this IQueryable<PersonalChatInfo> queryable,
        Guid firstUser,
        Guid secondUser)
        => queryable.FirstOrDefaultAsync(
            x =>
                (x.InitiatorPeer == firstUser && x.RecipientPeer == secondUser)
                || (x.InitiatorPeer == secondUser && x.RecipientPeer == firstUser));

    public static IQueryable<PersonalChatInfo> FilterByPeer(this IQueryable<PersonalChatInfo> queryable, Guid id)
        => queryable.Where(x => x.InitiatorPeer == id || x.RecipientPeer == id);
}
