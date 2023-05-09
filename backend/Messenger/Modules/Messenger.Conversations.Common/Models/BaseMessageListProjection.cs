using Messenger.Core.Model.ConversationAggregate.Attachment;

namespace Messenger.Conversations.Common.Models;

public abstract record BaseMessageListProjection(
    Guid MessageId,
    Guid SentBy,
    string Content,
    IReadOnlyCollection<IAttachment>? Attachments,
    DateTime SentAt,
    DateTime? EditedAt,
    uint Position);