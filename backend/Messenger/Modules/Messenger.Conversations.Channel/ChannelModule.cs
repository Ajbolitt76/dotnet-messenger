using Messenger.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Conversations.Channel;

public class ChannelModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
    }
}
