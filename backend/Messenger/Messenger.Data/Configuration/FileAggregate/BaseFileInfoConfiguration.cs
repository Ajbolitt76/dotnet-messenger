using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Messenger.Core.Model.FileAggregate;

namespace Messenger.Data.Configuration.FileAggregate;

public abstract class BaseFileInfoConfiguration<TEntity> : BaseConfiguration<TEntity>
    where TEntity : BaseFileInfo
{
    public override void ConfigureChild(EntityTypeBuilder<TEntity> typeBuilder)
    {
        typeBuilder
            .HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedById);

        typeBuilder.Property(x => x.CreatedAt)
            .IsRequired();

        typeBuilder.Property(x => x.FileName)
            .IsRequired();
    }
}
