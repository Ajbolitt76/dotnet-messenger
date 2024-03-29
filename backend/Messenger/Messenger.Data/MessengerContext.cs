﻿using System.Reflection;
using MediatR;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Data.Configuration;
using Messenger.Data.Configuration.ConversationAggregate.Members;
using Messenger.Data.Extensions;
using Messenger.Infrastructure.User;

namespace Messenger.Data;

public class MessengerContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IDbContext
{
    private readonly IMediator _mediator;
    private readonly IEnumerable<DependencyInjectedEntityConfiguration> _configurations;
    private IDbContextTransaction? _transaction;

    public DbSet<MessengerUser> MessengerUsers { get; private set; }

    public DbSet<SystemFile> Files { get; private set; }

    public DbSet<UploadingFile> UploadingFiles { get; private set; }
    
    public DbSet<Conversation> Conversations { get; private set; }
    
    public DbSet<ConversationMessage> ConversationMessages { get; private set; }
    
    public DbSet<ConversationUserStatus> ConversationUserStatuses { get; private set; }

    public DbSet<GroupChatInfo> GroupChatInfos { get; private set; }
    
    public DbSet<PersonalChatInfo> PersonalChatInfos { get; private set; }
    
    public DbSet<ChannelInfo> ChannelInfos { get; private set; }

    public DbSet<GroupChatMember> GroupChatMembers { get; private set; }
    
    public DbSet<ChannelMember> ChannelMembers { get; private set; }

    public DbSet<PersonalChatMember> PersonalChatMembers { get; private set; }

    public MessengerContext(
        DbContextOptions<MessengerContext> options,
        IMediator mediator,
        IEnumerable<DependencyInjectedEntityConfiguration> configurations) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _configurations = configurations;
    }

    protected void RegisterEnums(ModelBuilder builder)
    {
        // builder.HasPostgresEnum<Gender>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<BaseConversationInfo>();
        builder.Ignore<BaseMember>();

        base.OnModelCreating(builder);
        RegisterEnums(builder);

        foreach (var entityTypeConfiguration in _configurations)
            entityTypeConfiguration.Configure(builder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this, cancellationToken: cancellationToken);

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
