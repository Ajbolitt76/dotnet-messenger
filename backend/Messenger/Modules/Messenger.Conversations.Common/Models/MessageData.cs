using Messenger.Core.Model.ConversationAggregate.Attachment;

namespace Messenger.Conversations.Common.Models;

public record MessageData(
    string? TextContent = "",
    List<IAttachment>? Attachments = null,
    MessageForwardMode ForwardMode = MessageForwardMode.None,
    Guid? OriginalMessageId = null);
