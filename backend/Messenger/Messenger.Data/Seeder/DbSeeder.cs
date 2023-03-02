using Messenger.Core.Model;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Data.Seeder;

public class DbSeeder : IDbSeeder
{
    private readonly Random _random = new();

    public async Task Seed(IDbContext dbContext)
    {
        await SeedMessengerUsers(dbContext);
        await dbContext.SaveEntitiesAsync();
    }

    private async Task SeedMessengerUsers(IDbContext dbContext)
    {
        var users = new List<MessengerUser>
        {
            new()
            {
                UserName = "Storklovin",
                Name = "Артем Бутянов",
                PhoneNumber = $"+79{string.Join("", Enumerable.Range(1, 9).Select(_ => _random.Next(0, 9)))}",
                DateOfBirth = new DateTime(_random.Next(1960, 2009), _random.Next(1, 13), _random.Next(1, 29))
                    .ToUniversalTime(),
                IdentityUserId = new Guid("a293d683-cf18-41db-a1d3-fcc4ce65b306")
            },
            new()
            {
                UserName = "ShaltayAbc",
                Name = "Adel",
                PhoneNumber = $"+79{string.Join("", Enumerable.Range(1, 9).Select(_ => _random.Next(0, 9)))}",
                DateOfBirth = new DateTime(_random.Next(1960, 2009), _random.Next(1, 13), _random.Next(1, 29))
                    .ToUniversalTime(),
                IdentityUserId = new Guid("aa1c3fe1-2b51-4167-9e4f-951df3e1e34b")
            },
            new()
            {
                UserName = "W1ngshot",
                Name = "Kirill Samsonov",
                PhoneNumber = $"+79{string.Join("", Enumerable.Range(1, 9).Select(_ => _random.Next(0, 9)))}",
                DateOfBirth = new DateTime(_random.Next(1960, 2009), _random.Next(1, 13), _random.Next(1, 29))
                    .ToUniversalTime(),
                IdentityUserId = new Guid("5e441965-c5d8-4cfb-bc5c-38b8b9ef3cae")
            },
            new()
            {
                UserName = "AdmiralXy",
                Name = "Danil",
                PhoneNumber = $"+79{string.Join("", Enumerable.Range(1, 9).Select(_ => _random.Next(0, 9)))}",
                DateOfBirth = new DateTime(_random.Next(1960, 2009), _random.Next(1, 13), _random.Next(1, 29))
                    .ToUniversalTime(),
                IdentityUserId = new Guid("6cb83983-f9bd-49c7-b355-6d27abfc759a")
            },
            new()
            {
                UserName = "DungeonMaster",
                Name = "Alex Nechaev",
                PhoneNumber = $"+79{string.Join("", Enumerable.Range(1, 9).Select(_ => _random.Next(0, 9)))}",
                DateOfBirth = new DateTime(_random.Next(1960, 2009), _random.Next(1, 13), _random.Next(1, 29))
                    .ToUniversalTime(),
                IdentityUserId = new Guid("534ae679-a3a5-4e96-95c8-94881c6b0076")
            }
        };

        users.ForEach(
            user => typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))?.SetValue(user, user.IdentityUserId));
        users.ForEach(
            user => typeof(MessengerUser).GetProperty(nameof(MessengerUser.IdentityUserId))
                ?.SetValue(user, Guid.NewGuid()));
        await dbContext.MessengerUsers.AddRangeAsync(users);
    }

}

