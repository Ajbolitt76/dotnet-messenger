global using OneOf;
using Mapster;
using Messenger.Api.Configuration;
using Messenger.Api.Cors;
using Messenger.Api.Middleware;
using Messenger.Api.Modules;
using Messenger.Api.Swagger;
using Messenger.Api.Validation;
using Messenger.Auth;
using Messenger.Crypto;
using Messenger.Data;
using Messenger.Data.Seeder;
using Messenger.Files;
using Messenger.Files.Shared;
using Messenger.Infrastructure;
using Messenger.Infrastructure.Json;
using Messenger.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCustomLogging();

var maps = new TypeAdapterConfig();

var modules = new ModuleRegistrarBuilder()
    .AddModule<DataModule>()
    .AddModule<CryptoModule>()
    .AddModule<FileModule>()
    .AddModule<S3Module>()
    .AddModule<FileCoreModule>()
    .SetTypeAdapter(maps)
    .Build();

builder.Services
    .RegisterInfrastructureServices(builder.Configuration)
    .AddSingleton<TypeAdapterConfig>(maps)
    .AddCustomSwagger()
    .AddCoreServices()
    .AddAuthorization()
    .AddCorsConfiguration()
    .ConfigureJsonOptions(builder.Environment)
    .AddScoped<DbSeeder>();

builder.Services.Configure<ForwardedHeadersOptions>(
    options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

modules.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddCustomAuthentication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseForwardedHeaders();
app.UseCorsConfiguration(app.Environment);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

modules.MapRoutes(app);

app.UseAuthentication();

app.UseSerilogRequestLogging();

app.UseAuthorization();

ValidationCodeLocalized.SetRepetValidationErrorCodes();

app.Run();