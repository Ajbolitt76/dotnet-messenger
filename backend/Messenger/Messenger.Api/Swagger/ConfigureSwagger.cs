using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Messenger.Infrastructure.Extensions;
using Messenger.Infrastructure.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Messenger.Api.Swagger;

public static class ConfigureSwagger
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RepetApi", Version = "v1" });

                c.AddSecurityDefinition(
                    "bearerAuth",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "JWT Authorization header using the Bearer scheme."
                    });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                    { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                            },
                            new string[] { }
                        }
                    });

                c.UseOneOfForPolymorphism();
                var xmlFilename = AppContext.BaseDirectory;

                foreach (var xmlDoc in Directory.GetFiles(xmlFilename, "*.xml"))
                {
                    c.IncludeXmlComments(xmlDoc);
                }
                

                c.SchemaFilter<PolymorphicIntegrationSchemaFilter>();
            });
        
        services.ConfigureWithServices<SwaggerGenOptions>(
            (opts, sp) =>
            {
                opts.AddPolymorphicTypes(sp.GetRequiredService<IOptions<PolymorphismJsonOptions>>());
            });
        
        
        return services;
    }

    private static void AddPolymorphicTypes(this SwaggerGenOptions c, IOptions<PolymorphismJsonOptions> options)
    {
        var defs = options.Value.GetDefinitions();
        c.SelectSubTypesUsing(
            sub => 
                defs.TryGetValue(sub, out var definition)
                ? definition.DerivedTypes.Select(x => x.DerivedType).ToArray()
                : Array.Empty<Type>());
    }
}
