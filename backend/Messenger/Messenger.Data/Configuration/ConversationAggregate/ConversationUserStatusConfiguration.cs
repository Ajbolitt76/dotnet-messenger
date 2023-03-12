using Messenger.Core.Model.ConversationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.ConversationAggregate;

public class ConversationUserStatusConfiguration : BaseConfiguration<ConversationUserStatus>
{
    public override void ConfigureChild(EntityTypeBuilder<ConversationUserStatus> typeBuilder)
    {
        typeBuilder.Property(x => x.ReadTo);
        
        typeBuilder.Property(x => x.DeletedTo);
        
        typeBuilder.Property(x => x.IsDeletedByUser)
            .HasDefaultValue(false);
        
        typeBuilder.HasIndex(x => new { x.ConversationId, x.UserId })
            .IsDescending(false, false)
            .IsUnique();
    }
}
