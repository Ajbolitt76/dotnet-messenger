using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Data.Configuration;
using Messenger.Support.Api.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Support.Api.Data;

public class SupportDbContext : DbContext, IDbContext
{
    
    private readonly IEnumerable<DependencyInjectedEntityConfiguration> _configurations;
    public DbSet<ConversationMessage> ConversationMessages { get; }

    public SupportDbContext(DbContextOptions<SupportDbContext> options, 
        IEnumerable<DependencyInjectedEntityConfiguration> configurations)
    {
        _configurations = configurations;
    }
    
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return true;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<BaseConversationInfo>();
        builder.Ignore<BaseMember>();

        base.OnModelCreating(builder);
        
        foreach (var entityTypeConfiguration in _configurations)
            entityTypeConfiguration.Configure(builder);
    }
}
