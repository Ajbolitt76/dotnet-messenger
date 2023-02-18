using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Infrastructure.Extensions;


public static class RegisterEndpoints
{
    public static void AddEndpoints(this IEndpointRouteBuilder routeBuilder, IEnumerable<Assembly> assemblies)
    {
        var endpointsRoots = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(t => t.GetInterfaces().Contains(typeof(IEndpointRoot)))
            .Where(t => t is { IsInterface: false, IsAbstract: false });
        
        foreach (var endpointRoot in endpointsRoots)
        {
            var instance = (IEndpointRoot)ActivatorUtilities.CreateInstance(routeBuilder.ServiceProvider, endpointRoot);
            instance?.MapEndpoints(routeBuilder);
        }
    }
    
    public static IEndpointRouteBuilder AddEndpoint<T>(this IEndpointRouteBuilder routeBuilder)
        where T : IEndpoint
    {
        var instance = ActivatorUtilities.CreateInstance<T>(routeBuilder.ServiceProvider);
        instance?.Map(routeBuilder);
        return routeBuilder;
    }
}