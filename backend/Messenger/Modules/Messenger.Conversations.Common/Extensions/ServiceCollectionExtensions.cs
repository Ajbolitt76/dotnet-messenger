using Messenger.Conversations.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Messenger.Conversations.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static MessageHandlerRegistrar AddMessageHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<MessageHandlerProvider>();
        return new MessageHandlerRegistrar(serviceCollection);
    }
}
