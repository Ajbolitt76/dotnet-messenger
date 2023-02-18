using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Configuration;

public abstract class DependencyInjectedEntityConfiguration
{
    public abstract void Configure(ModelBuilder modelBuilder);
}

public abstract class DependencyInjectedEntityConfiguration<TEntity>
    : DependencyInjectedEntityConfiguration, IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public abstract void Configure(EntityTypeBuilder<TEntity> builder);

    public sealed override void Configure(ModelBuilder modelBuilder)
        => Configure(modelBuilder.Entity<TEntity>());
}
