using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Attachment;
using Messenger.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.ConversationAggregate;

public class ConversationMessageConfiguration : BaseConfiguration<ConversationMessage>
{
    public override void ConfigureChild(EntityTypeBuilder<ConversationMessage> typeBuilder)
    {
        typeBuilder.Property(x => x.Metadata)
            .HasJsonConversion();
        
        typeBuilder.SetPropertyAccessModeField(x => x.Attachments, ConversationMessage.AttachmentsFieldName);
        typeBuilder.Property(x => x.Attachments!)
            .HasJsonConversion<IReadOnlyList<IAttachment>, List<IAttachment>>();
    }
}
