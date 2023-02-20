using Messenger.Core.Model.ConversationAggregate.Attachment;

namespace Messenger.Core.Model.ConversationAggregate;

public class ConversationMessage : BaseEntity
{
    public const string AttachmentsFieldName = nameof(_attachments);
    
    private List<IAttachment>? _attachments;
    
    public string? TextContent { get; set; }

    public IReadOnlyList<IAttachment>? Attachments => _attachments?.AsReadOnly();

    public DateTime SentAt { get; set; }
    
    public DateTime EditedAt { get; set; }

    public ConversationMessageMetadata Metadata { get; set; } = ConversationMessageMetadata.Default;
}
