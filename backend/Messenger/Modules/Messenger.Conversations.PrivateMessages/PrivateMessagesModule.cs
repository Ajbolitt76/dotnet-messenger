using Messenger.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Conversations.PrivateMessages;

public class PrivateMessagesModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
    }
}
