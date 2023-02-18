using Mapster;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Messenger.Api.Configuration;
using Messenger.Api.Middleware;
using Messenger.Api.Modules;
using Messenger.Api.Swagger;
using Messenger.Api.Validation;
using Messenger.Auth;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Crypto;
using Messenger.Data;
using Messenger.Files;
using Messenger.Files.Shared;
using Messenger.Infrastructure;
using Messenger.RapiDoc;
using Messenger.User;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCustomLogging();

var maps = new TypeAdapterConfig();

var modules = new ModuleRegistrarBuilder()
    .AddModule<DataModule>()
    .AddModule<CryptoModule>()
    .AddModule<AuthModule>()
    .AddModule<FileModule>()
    .AddModule<FileCoreModule>()
    .AddModule<UserModule>()
    .SetTypeAdapter(maps)
    .Build();

builder.Services
    .RegisterInfrastructureServices(builder.Configuration)
    .AddSingleton<TypeAdapterConfig>(maps)
    .AddCustomSwagger()
    .AddCoreServices()
    .AddAuthorization()
    .AddCorsConfiguration()
    .ConfigureJsonOptions(builder.Environment);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

modules.RegisterServices(builder.Services, builder.Configuration); 

builder.Services.AddCustomAuthentication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseRapiDocUI(c =>
    {
        c.RoutePrefix = "swagger"; // serve the UI at root
        c.SwaggerEndpoint("v1/swagger.json", "V1 Docs");
        c.GenericRapiConfig = new GenericRapiConfig()
        {
            RenderStyle="focused",
            Layout = "column",
            FontSize = "large",
            Theme="light",//light,dark,focused   
        };
    });
}

try
{
    await using var scope = app.Services.CreateAsyncScope();
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<RepetContext>();
    await db.Database.MigrateAsync();

    await using var conn = (NpgsqlConnection)db.Database.GetDbConnection();
    await conn.OpenAsync();
    await conn.ReloadTypesAsync();
}
catch (Exception e)
{
    app.Logger.LogError(e, "Error while migrating the database");
    Environment.Exit(-1);
}

app.UseStaticFiles();

app.UseCorsConfiguration(app.Environment);

modules.MapRoutes(app);

app.UseAuthentication();

app.UseSerilogRequestLogging();

app.UseAuthorization();

ValidationCodeLocalized.SetRepetValidationErrorCodes();

app.Run();