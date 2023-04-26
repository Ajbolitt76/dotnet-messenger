using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Attachment;
using Messenger.Data.Extensions;
using Microsoft.EntityFrameworkCore;
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

        typeBuilder.Property(x => x.SentAt)
            .HasDefaultValueSql("NOW()");

        typeBuilder.HasOne(x => x.SenderMessengerUser)
            .WithMany()
            .HasForeignKey(x => x.SenderId);
        
        typeBuilder.HasIndex(x => new { x.ConversationId, x.Position })
            .IsDescending(false, false)
            .IsUnique();
    }
}
