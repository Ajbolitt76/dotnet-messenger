using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Infrastructure.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Messenger.Api.Swagger;

public class PolymorphicIntegrationSchemaFilter : ISchemaFilter
{
    private readonly struct DerivedTypeInformation
    {
        public readonly string DiscriminatorPropertyName;
        public readonly JsonDerivedType JsonDerivedType;

        public DerivedTypeInformation(string discriminatorPropertyName, JsonDerivedType jsonDerivedType)
        {
            DiscriminatorPropertyName = discriminatorPropertyName;
            JsonDerivedType = jsonDerivedType;
        }
    }

    private readonly JsonOptions _jsonOptions;
    private readonly IReadOnlyDictionary<Type, BasePolymorphicTypeDefinition> _polymorphic;
    private readonly IReadOnlyDictionary<Type, DerivedTypeInformation> _derivedTypesInfo;

    public PolymorphicIntegrationSchemaFilter(
        IOptions<PolymorphismJsonOptions> options,
        IOptions<JsonOptions> jsonOptions)
    {
        _jsonOptions = jsonOptions.Value;
        _polymorphic = options.Value.GetDefinitions();

        var derivedTypeInfo = new Dictionary<Type, DerivedTypeInformation>();

        foreach (var (_, def) in _polymorphic)
        foreach (var derivedTypeDef in def.DerivedTypes)
            derivedTypeInfo[derivedTypeDef.DerivedType] = new DerivedTypeInformation(
                def.DiscriminatorPropertyName,
                derivedTypeDef);

        _derivedTypesInfo = derivedTypeInfo;
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (_polymorphic.TryGetValue(context.Type, out var def))
        {
            var derivedSchemas = def
                .DerivedTypes
                .ToDictionary(
                    x =>
                        x.TypeDiscriminator.ToString(),
                    x => context
                        .SchemaGenerator
                        .GenerateSchema(x.DerivedType, context.SchemaRepository));

            schema.Discriminator = new OpenApiDiscriminator
            {
                PropertyName = def.DiscriminatorPropertyName,
                Mapping = derivedSchemas
                    .ToDictionary(x => x.Key, x => x.Value.Reference.ReferenceV3)
            };
        }

        if (_derivedTypesInfo.TryGetValue(context.Type, out var derivedTypeInfo))
        {
            var derivedType = derivedTypeInfo.JsonDerivedType;
            var discriminatorPropertyName = derivedTypeInfo.DiscriminatorPropertyName;

            var discriminatorValue = derivedType.TypeDiscriminator.ToString();
            var discriminatorProperty = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString(discriminatorValue),
                Enum = new List<IOpenApiAny>()
                {
                    new OpenApiString(discriminatorValue)
                }
            };

            schema.Properties[discriminatorPropertyName] = discriminatorProperty;
            schema.Required.Add(discriminatorPropertyName);
        }
    }
}
