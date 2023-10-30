using Messenger.Core.Services;
using Messenger.Infrastructure.Services;
using Messenger.Support.Api.MessageActions.SupportStoreMessage;

namespace Messenger.Support.Api.Configurations;

public static class InfrastructureConfiguration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ISupportStoreMessageActionHandler, SupportStoreMessageActionHandler>();
        
        return services;
    }
}
