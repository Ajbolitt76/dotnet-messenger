using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Modules;

public interface IModuleRegistrar
{
    public void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration);

    public void MapRoutes(IEndpointRouteBuilder routeBuilder);
}
