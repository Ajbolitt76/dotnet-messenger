using Messenger.Conversations.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Messenger.Conversations.Common.Services;

public class MessageHandlerProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MessageHandlerTypesStore _messageHandlers;

    public MessageHandlerProvider(IOptions<MessageHandlerTypesStore> messageHandlers, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _messageHandlers = messageHandlers.Value;
    }

    public IMessageActionHandler<TAction, TResult>? GetMessageHandler<TAction, TResult>(string messageType)
        where TAction : class, IMessageAction<TResult>
    {
        var messageHandlerType = _messageHandlers.GetHandlerTypeByDiscriminator<TAction, TResult>(messageType);

        if (messageHandlerType is null)
            return null;

        var resolvedService = _serviceProvider.GetService(messageHandlerType);

        return resolvedService is IMessageActionHandler<TAction, TResult> messageHandler
            ? messageHandler
            : throw new InvalidOperationException(
                $"Некорректный тип обработчика в контенере для сообщения {messageType}. Получено: {resolvedService?.GetType().FullName}");
    }
}
