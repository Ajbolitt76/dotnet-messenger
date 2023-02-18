using Microsoft.EntityFrameworkCore;
using Messenger.Core.Model.FileAggregate;
using Messenger.Core.Model.UserAggregate;

namespace Messenger.Core.Requests.Abstractions;

public interface IDbContext
{
    public DbSet<RepetUser> RepetUsers { get; set; }
    
    public DbSet<SystemFile> Files { get; set; }
    
    public DbSet<UploadingFile> UploadingFiles { get; set; }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
