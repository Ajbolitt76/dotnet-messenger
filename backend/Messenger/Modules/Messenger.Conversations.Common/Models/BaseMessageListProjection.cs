namespace Messenger.Conversations.Common.Models;

public abstract record BaseMessageListProjection(
    Guid MessageId,
    DateTime SentAt,
    DateTime? EditedAt);