using Messenger.Conversations.Common.Abstractions;

namespace Messenger.Conversations.Common.Services;

public class MessageHandlerTypesStore
{
    private Dictionary<string, Dictionary<Type, Type>> _handlers = new();

    public MessageHandlerTypesStore()
    {
    }

    internal IReadOnlyDictionary<string, Dictionary<Type, Type>> Handlers
    {
        get => _handlers;
        set
        {
            var wrongTypes = _handlers
                .SelectMany(x => x.Value.Select(y => y.Value))
                .Where(
                    x => x.IsAbstract
                        || x.GetInterfaces()
                            .All(y => y.GetGenericTypeDefinition() != typeof(IMessageActionHandler<,>)))
                .ToList();

            if (wrongTypes.Count > 0)
                throw new InvalidOperationException(
                    $"Попытка зарегестрировать неверные обработчики:\n{string.Join("\n", wrongTypes.Select(x => x.FullName))}");

            _handlers = value.ToDictionary(x => x.Key, x => x.Value);
        }
    }

    public Type? GetHandlerTypeByDiscriminator<TAction, TResult>(string discriminator)
        where TAction : IMessageAction<TResult>
        => _handlers
            .GetValueOrDefault(discriminator)
            ?.GetValueOrDefault(typeof(TAction));
}
