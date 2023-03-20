using Messenger.Core.Model.ConversationAggregate.Attachment;
using Microsoft.VisualBasic;

namespace Messenger.Core.Model.ConversationAggregate;

public class ConversationMessage : BaseEntity
{
    public const string AttachmentsFieldName = nameof(_attachments);

    private List<IAttachment>? _attachments;

    public Guid? ConversationId { get; set; }

    public Guid? SenderId { get; set; }

    public string? TextContent { get; set; }
    
    public List<Guid>? DeletedFrom { get; set; }

    public IReadOnlyList<IAttachment>? Attachments
    {
        get => _attachments;
        set => _attachments = value != null
            ? new List<IAttachment>(value)
            : null;
    }

    public DateTime SentAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public uint Position { get; set; }

    public ConversationMessageMetadata Metadata { get; set; } = ConversationMessageMetadata.Default;
}
