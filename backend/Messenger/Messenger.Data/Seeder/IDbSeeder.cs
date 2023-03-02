using Messenger.Core.Requests.Abstractions;

namespace Messenger.Data.Seeder;

public interface IDbSeeder
{
    Task Seed(IDbContext dbContext);
}
