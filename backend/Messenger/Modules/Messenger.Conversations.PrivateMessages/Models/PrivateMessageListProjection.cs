using Messenger.Conversations.Common.Models;
using Messenger.Core;
using Messenger.Core.Model.ConversationAggregate;

namespace Messenger.Conversations.PrivateMessages.Models;

public record PrivateMessageListProjection(
        Guid MessageId,
        DateTime SentAt,
        DateTime? EditedAt)
    : BaseMessageListProjection(MessageId, SentAt, EditedAt), IHaveDiscriminator
{
    public static string Discriminator => ConversationTypes.PersonalChat;
}
