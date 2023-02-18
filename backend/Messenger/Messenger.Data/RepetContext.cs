using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Data.Configuration;
using Messenger.Data.Extensions;
using Messenger.Infrastructure.User;

namespace Messenger.Data;

public class RepetContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IDbContext
{
    private readonly IMediator _mediator;
    private readonly IEnumerable<DependencyInjectedEntityConfiguration> _configurations;
    private IDbContextTransaction? _transaction;

    public DbSet<RepetUser> RepetUsers { get; set; }
    
    public DbSet<SystemFile> Files { get; set; }
    
    public DbSet<UploadingFile> UploadingFiles { get; set; }

    public RepetContext(
        DbContextOptions<RepetContext> options,
        IMediator mediator,
        IEnumerable<DependencyInjectedEntityConfiguration> configurations) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _configurations = configurations;
    }

    protected void RegisterEnums(ModelBuilder builder)
    {
        builder.HasPostgresEnum<UserAccountType>();
        builder.HasPostgresEnum<Gender>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        RegisterEnums(builder);
        
        foreach (var entityTypeConfiguration in _configurations) 
            entityTypeConfiguration.Configure(builder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);

        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override void Dispose()
    {
        _transaction?.Dispose();
        base.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        if (_transaction != null)
            await _transaction.DisposeAsync();
        await base.DisposeAsync();
    }
}
