using Messenger.Core.Model.ConversationAggregate.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration.ConversationAggregate.Members;

public abstract class BaseMemberConfiguration<TEntity> : BaseConfiguration<TEntity>
    where TEntity : BaseMember
{
    public override void ConfigureChild(EntityTypeBuilder<TEntity> typeBuilder)
    {
        typeBuilder.HasOne(x => x.Conversation)
            .WithMany();
    }
}
