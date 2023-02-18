using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Modules;

public interface IModule
{
    public static abstract void RegisterModule(IServiceCollection services, IConfiguration configuration);
}
