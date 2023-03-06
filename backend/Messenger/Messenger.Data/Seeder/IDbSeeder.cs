using Messenger.Core.Requests.Abstractions;

namespace Messenger.Data.Seeder;

public interface IDbSeeder
{
    Task SeedAsync(IDbContext dbContext, IServiceProvider serviceProvider);
}
