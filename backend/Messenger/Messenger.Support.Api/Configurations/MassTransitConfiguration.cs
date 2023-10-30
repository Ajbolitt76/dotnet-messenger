using MassTransit;
using Messenger.Support.Api.Consumers;

namespace Messenger.Support.Api.Configurations;

public static class MassTransitConfiguration
{
    public static void AddMassTransitConfiguration(this WebApplicationBuilder builder)
    {
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

                configurator.AddConsumer<SupportStoreMessagesConsumer>();
            });
    }
}
