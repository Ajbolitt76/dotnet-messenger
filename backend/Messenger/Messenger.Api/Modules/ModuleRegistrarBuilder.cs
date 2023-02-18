using System.Reflection;
using FluentValidation;
using Mapster;
using MediatR;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;
using Messenger.Infrastructure.Modules;

namespace Messenger.Api.Modules;

public class ModuleRegistrarBuilder : IModuleRegistrarBuilder<ModuleRegistrarBuilder>
{
    private List<Action<IServiceCollection, IConfiguration>> _addToServices = new();
    private List<Assembly> _assemblies = new();
    private TypeAdapterConfig? _typeAdapterConfig;

    public ModuleRegistrarBuilder AddModule<TModule>() where TModule : IModule
    {
        _assemblies.Add(typeof(TModule).Assembly);
        _addToServices.Add(TModule.RegisterModule);
        return this;
    }

    public ModuleRegistrarBuilder SetTypeAdapter(TypeAdapterConfig adapterConfig)
    {
        _typeAdapterConfig = adapterConfig;
        return this;
    }

    public IModuleRegistrar Build()
        => new ModuleRegistrar(_typeAdapterConfig, _addToServices.ToArray(), _assemblies.ToArray());
}
