using MassTransit;
using Messenger.Support.Consumers;
using Messenger.Support.MessageActions.SupportSendMessage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Support.Bootstrap;

public static class MassTransitBootstrap
{
    public static WebApplicationBuilder AddMassTransitBootstrap(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISupportSendMessageActionHandler, SupportSendMessageActionHandler>();
        
        builder.Services.AddMassTransit(
            configurator =>
            {
                configurator.SetKebabCaseEndpointNameFormatter();
                configurator.UsingRabbitMq(
                    (context, config) =>
                    {
                        config.Host(new Uri(builder.Configuration["MessageBroker:Host"]!),
                            h =>
                            {
                                h.Username(builder.Configuration["MessageBroker:Username"]);
                                h.Username(builder.Configuration["MessageBroker:Password"]);
                            });
                
                        config.ConfigureEndpoints(context);
                    });
                
                configurator.AddConsumer<SupportSendMessagesConsumer>();
            });
        return builder;
    }
}
