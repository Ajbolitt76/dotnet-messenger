using System.Reflection;
using MassTransit;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Data;
using Messenger.Data.Configuration;
using Messenger.Data.Configuration.UserAggregate;
using Messenger.Support.Api.Consumers;
using Messenger.Support.Api.MessageActions.SupportStoreMessage;
using Microsoft.EntityFrameworkCore;
using Messenger.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

services.Scan(
    scan => scan
        .FromAssembliesOf(typeof(MessngerUserConfiguration))
        .AddClasses(classes => classes.AssignableTo(typeof(DependencyInjectedEntityConfiguration)))
        .As<DependencyInjectedEntityConfiguration>()
        .WithSingletonLifetime());

services.AddDbContext<IDbContext, MessengerContext>(
    options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DataConnectionString"),
            builder =>
            {
                builder.MigrationsAssembly(typeof(MessengerContext).GetTypeInfo().Assembly.GetName().Name);
                builder.EnableRetryOnFailure(
                    maxRetryCount: 15,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
    });



builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddScoped<ISupportStoreMessageActionHandler, SupportStoreMessageActionHandler>();

services.AddMassTransit(
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

var app = builder.Build();

app.Run();