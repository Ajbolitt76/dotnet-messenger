using MassTransit;
using Messenger.Support.MessageActions.SupportSendMessage;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Support.Bootstrap;

public static class MassTransitBootstrap
{
    public static IServiceCollection AddMassTransitBootstrap(this IServiceCollection services)
    {
        services.AddMassTransit(
            x =>
            {
                x.AddConsumers(typeof(MassTransitBootstrap).Assembly);
                x.UsingInMemory(
                    (context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });
            });

        services.AddScoped<ISupportSendMessageActionHandler, SupportSendMessageActionHandler>();
        
        return services;
    }
}
