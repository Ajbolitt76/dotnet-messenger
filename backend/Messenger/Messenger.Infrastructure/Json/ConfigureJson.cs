using System.Text.Json;
using System.Text.Json.Serialization;
using Messenger.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Json;

public static class ConfigureJson
{
    public static IServiceCollection ConfigureJsonOptions(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services.ConfigureWithServices<JsonOptions>(
            (opts, sp) => SetJsonProperties(
                opts.SerializerOptions,
                environment,
                sp.GetRequiredService<IOptions<PolymorphismJsonOptions>>()));

        services.ConfigureWithServices<Microsoft.AspNetCore.Mvc.JsonOptions>(
            (opts, sp) => SetJsonProperties(
                opts.JsonSerializerOptions,
                environment,
                sp.GetRequiredService<IOptions<PolymorphismJsonOptions>>()));

        services.ConfigureWithServices<JsonSerializerOptions>(
            (opts, sp) => SetJsonProperties(
                opts,
                environment,
                sp.GetRequiredService<IOptions<PolymorphismJsonOptions>>()));
        
        services.ConfigureWithServices<JsonHubProtocolOptions>(
            (opts, sp) => SetJsonProperties(
                opts.PayloadSerializerOptions,
                environment,
                sp.GetRequiredService<IOptions<PolymorphismJsonOptions>>()));

        return services;
    }

    public static void SetJsonProperties(
        JsonSerializerOptions options,
        IWebHostEnvironment environment,
        IOptions<PolymorphismJsonOptions> polyOpts)
    {
        if (!(environment.IsProduction() || environment.IsStaging()))
        {
            options.WriteIndented = true;
        }

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.TypeInfoResolver = new PolymorphicTypeResolver(polyOpts);
    }
}
