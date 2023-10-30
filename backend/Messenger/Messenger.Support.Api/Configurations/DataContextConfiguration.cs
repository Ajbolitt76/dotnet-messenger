using System.Reflection;
using Messenger.Core.Requests.Abstractions;
using Messenger.Data;
using Messenger.Data.Configuration;
using Messenger.Data.Configuration.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Support.Api.Configurations;

public static class DataContextConfiguration
{
    public static void AddDataContextConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(
            scan => scan
                .FromAssembliesOf(typeof(MessngerUserConfiguration))
                .AddClasses(classes => classes.AssignableTo(typeof(DependencyInjectedEntityConfiguration)))
                .As<DependencyInjectedEntityConfiguration>()
                .WithSingletonLifetime());

        builder.Services.AddDbContext<IDbContext, MessengerContext>(
            options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DataConnectionString"),
                    builder =>
                    {
                        builder.MigrationsAssembly(typeof(MessengerContext).GetTypeInfo().Assembly.GetName().Name);
                        builder.EnableRetryOnFailure(
                            maxRetryCount: 15,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    });
            });
    }
}
