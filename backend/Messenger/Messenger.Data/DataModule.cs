using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Data.Configuration;
using Messenger.Data.Configuration.UserAggregate;
using Messenger.Infrastructure.Json;
using Messenger.Infrastructure.Modules;
using Messenger.Infrastructure.User;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace Messenger.Data;

public class DataModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Gender>();
        
        RegisterJsonPolymorphicDefinitions(services);
        
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<MessengerContext>();

        services.AddDbContext<IDbContext, MessengerContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DataConnectionString"),
                builder =>
                {
                    builder.MigrationsAssembly(typeof(MessengerContext).GetTypeInfo().Assembly.GetName().Name);
                    builder.EnableRetryOnFailure(
                        maxRetryCount: 15,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });
        });

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(RepetUserConfiguration))
            .AddClasses(classes => classes.AssignableTo(typeof(DependencyInjectedEntityConfiguration)))
            .As<DependencyInjectedEntityConfiguration>()
            .WithSingletonLifetime());
        
        services.AddStackExchangeRedisExtensions<SystemTextJsonSerializer>(options =>
        {
            return new RedisConfiguration[]
            {
                new()
                {
                    ConnectionString = configuration.GetConnectionString("RedisConnectionString"),
                }
            };
        });
    }

    private static IServiceCollection RegisterJsonPolymorphicDefinitions(IServiceCollection serviceCollection)
    {
        serviceCollection.Configure<PolymorphismJsonOptions>(
            poly =>
            {  
                poly.AddTypeDefinition<IFileLocation, TusFileLocation>();
                poly.AddTypeDefinition<IFileLocation, S3FileLocation>();
            });

        return serviceCollection;
    }
}