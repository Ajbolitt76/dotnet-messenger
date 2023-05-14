using Messenger.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Subscriptions;

public class SubscriptionsModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
    }
}
