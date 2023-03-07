using Messenger.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Conversations.GroupChats;

public class GroupChatsModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
    }
}
