using Messenger.Conversations.Common.Models;
using Messenger.Core;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Attachment;

namespace Messenger.Conversations.PrivateMessages.Models;

public record PrivateMessageListProjection(
        Guid MessageId,
        string Content,
        IReadOnlyCollection<IAttachment>? Attachments,
        DateTime SentAt,
        DateTime? EditedAt,
        uint Position)
    : BaseMessageListProjection(MessageId, Content, Attachments, SentAt, EditedAt, Position), IHaveDiscriminator
{
    public static string Discriminator => ConversationTypes.PersonalChat;
}
