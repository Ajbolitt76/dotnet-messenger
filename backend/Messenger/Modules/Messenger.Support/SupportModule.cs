using Messenger.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Support;

public class SupportModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
    }
}
