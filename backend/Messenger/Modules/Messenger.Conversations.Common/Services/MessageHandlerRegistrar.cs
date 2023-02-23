using Messenger.Conversations.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Conversations.Common.Services;

public class MessageHandlerRegistrar
{
    private readonly IServiceCollection _sc;
    
    // Discriminator : [Action -> ActionHandler]
    private readonly Dictionary<string, Dictionary<Type, Type>> _handlers = new();

    public MessageHandlerRegistrar(IServiceCollection sc)
        => _sc = sc;

    public MessageHandlerRegistrar AddHandler<T, TAction, TResult>()
        where T : IMessageActionHandler<TAction, TResult> where TAction : class, IMessageAction<TResult>
    {
        if (!_handlers.ContainsKey(T.MessageType))
            _handlers[T.MessageType] = new Dictionary<Type, Type>()
            {
                [typeof(TAction)] = typeof(T)
            };
        else
            _handlers[T.MessageType].Add(typeof(TAction), typeof(T));
        
        return this;
    }

    public IServiceCollection Apply()
    {
        _sc.Configure<MessageHandlerTypesStore>(
            ms => { ms.Handlers = _handlers; });

        foreach (var (_, messageActions) in _handlers)
            foreach (var (_, actionHandler) in messageActions)
                _sc.AddScoped(actionHandler);
        
        return _sc;
    }
}
