using Messenger.Support.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

builder.AddDataContextConfiguration();

services.ConfigureInfrastructure();

builder.AddMassTransitConfiguration();

var app = builder.Build();

app.Run();