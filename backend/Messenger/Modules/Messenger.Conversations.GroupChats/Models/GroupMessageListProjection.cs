﻿using Messenger.Conversations.Common.Models;
using Messenger.Core;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Attachment;

namespace Messenger.Conversations.GroupChats.Models;

public record GroupMessageListProjection(
        Guid MessageId,
        Guid SentBy,
        string Content,
        IReadOnlyCollection<IAttachment>? Attachments,
        DateTime SentAt,
        DateTime? EditedAt,
        uint Positon)
    : BaseMessageListProjection(MessageId, SentBy, Content, Attachments, SentAt, EditedAt, Positon), IHaveDiscriminator
{
    public static string Discriminator => ConversationTypes.GroupChat;
};
