using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.ConversationAggregate.ConversationInfos;

public abstract class BaseConversationInfoConfiguration<TEntity> : BaseConfiguration<TEntity>
    where TEntity : BaseConversationInfo
{
    public override void ConfigureChild(EntityTypeBuilder<TEntity> typeBuilder)
    {
        typeBuilder.HasOne(x => x.Conversation)
            .WithOne()
            .HasForeignKey<TEntity>(x => x.ConversationId);

        typeBuilder.HasIndex(x => new { x.ConversationId, x.LastUpdated })
            .IsDescending(false, true);
    }
}