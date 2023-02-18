namespace Messenger.Infrastructure.Modules;

public interface IModuleRegistrarBuilder<out T> where T : IModuleRegistrarBuilder<T>
{
    public T AddModule<TModule>() where TModule : IModule;
    
    public IModuleRegistrar Build();
}
