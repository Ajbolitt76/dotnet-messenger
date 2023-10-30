using Messenger.Core.Model.ConversationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Support.Api.Data.Abstractions;

public interface IDbContext
{
    DbSet<ConversationMessage> ConversationMessages { get; }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
