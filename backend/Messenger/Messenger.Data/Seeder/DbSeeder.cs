using Messenger.Core.Model;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Data.Seeder;

public class DbSeeder : IDbSeeder
{
    private readonly Random _random = new();

    public async Task SeedAsync(IDbContext dbContext, IServiceProvider serviceProvider)
    {
        await SeedMessengerUsers(dbContext, serviceProvider);
        await dbContext.SaveEntitiesAsync();
    }

    private async Task SeedMessengerUsers(IDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (!dbContext.MessengerUsers.Any(x => x.Id == Guid.Empty))
        {
            dbContext.MessengerUsers.Add(
                new MessengerUser()
                {
                    UserName = "System",
                    Name = "System",
                    PhoneNumber = "System",
                    IdentityUserId = Guid.Empty,
                });
            await dbContext.SaveEntitiesAsync();
        }
        
        if (dbContext.MessengerUsers.Any())
            return;
        
        var userService = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        var appUsers = new (string Name, Guid Id)[] 
        {
            ("Adel", new Guid("aa1c3fe1-2b51-4167-9e4f-951df3e1e34b")),
            ("Artem", new Guid("a293d683-cf18-41db-a1d3-fcc4ce65b306")),
            ("Kirill", new Guid("5e441965-c5d8-4cfb-bc5c-38b8b9ef3cae")),
            ("Nastya", new Guid("534ae679-a3a5-4e96-95c8-94881c6b0076"))
        }.Select(
            x =>
            {
                var data = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = x.Name,
                    Email = $"{x}@mail.ru",
                    PhoneNumber = $"+79{string.Join("", Enumerable.Range(1, 9).Select(_ => _random.Next(0, 9)))}",
                };
                data.PasswordHash = userService.PasswordHasher.HashPassword(data, x.Name);
                return (data, x.Id);
            }).ToArray();
        
        foreach (var appUser in appUsers)
        {
            await userService.CreateAsync(appUser.data);
            
            var messengerUser = new MessengerUser
            {
                UserName =  appUser.data.UserName!,
                PhoneNumber = appUser.data.PhoneNumber!,
                Name = appUser.data.UserName!,
                DateOfBirth = new DateTime(_random.Next(1960, 2009), _random.Next(1, 13), _random.Next(1, 29)).ToUniversalTime(),
                IdentityUserId = appUser.data.Id,
            };
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))?.SetValue(messengerUser, appUser.Id);
            dbContext.MessengerUsers.Add(messengerUser);
        }
    }

}

