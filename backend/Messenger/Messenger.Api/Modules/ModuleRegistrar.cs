using System.Reflection;
using FluentValidation;
using Mapster;
using MediatR;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;
using Messenger.Infrastructure.Modules;
using Messenger.Infrastructure.Validation;

namespace Messenger.Api.Modules;

public class ModuleRegistrar : IModuleRegistrar
{
    private readonly TypeAdapterConfig? _typeAdapterConfig;
    private readonly Action<IServiceCollection, IConfiguration>[] _actions;
    private readonly Assembly[] _assemblies;

    public ModuleRegistrar(
        TypeAdapterConfig? typeAdapterConfig, 
        Action<IServiceCollection, IConfiguration>[] actions,
        Assembly[] assemblies)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _actions = actions;
        _assemblies = assemblies;
    }
    
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var register in _actions) 
            register(services, configuration);
        
        services.AddMediatR(_assemblies);
        
        services.Scan(
            scan =>
            {
                scan.FromAssemblies(_assemblies)
                    .AddClasses(x => x.AssignableTo(typeof(IDomainHandler<,>)))
                    .AsImplementedInterfaces(
                        x => x.GetGenericTypeDefinition() == typeof(IDomainHandler<,>))
                    
                    .AddClasses(x => x
                        .AssignableTo(typeof(IValidator<>))
                        .WithoutAttribute<DoNotAutoRegisterAttribute>())
                    .AsImplementedInterfaces(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .WithScopedLifetime();
            });

        _typeAdapterConfig?.Scan(_assemblies);
    }
    
    public void MapRoutes(IEndpointRouteBuilder applicationBuilder) 
        => applicationBuilder.AddEndpoints(_assemblies);
}
