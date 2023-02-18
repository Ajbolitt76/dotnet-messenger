using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Messenger.Infrastructure.Extensions;
using Messenger.Infrastructure.Json;

namespace Messenger.Api.Configuration;

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

        return services;
    }

    private static void SetJsonProperties(
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
