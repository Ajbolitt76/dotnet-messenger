using Messenger.Infrastructure.Json;
using Messenger.Infrastructure.Modules;
using Messenger.RealTime.Common.Services;
using Messenger.RealTime.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.RealTime;

public class RealtimeModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR()
            .AddJsonProtocol();
        services.AddSingleton<IUpdateConnectionManager, UpdateConnectionManager>();
        services.AddSingleton<IUserUpdateWorkerFactory, UserUpdateWorkerFactory>();
    }
}
