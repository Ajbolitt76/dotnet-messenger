using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Json;

public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    private readonly PolymorphismJsonOptions _config;
    
    public PolymorphicTypeResolver(IOptions<PolymorphismJsonOptions> options) => _config = options.Value;

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        var polymorphicOptions = _config.GetDefinition(jsonTypeInfo.Type);

        if (polymorphicOptions != null) jsonTypeInfo.PolymorphismOptions = polymorphicOptions;

        return jsonTypeInfo;
    }
}
