using System.Text.Json.Serialization.Metadata;

namespace Messenger.Infrastructure.Json;

public record BasePolymorphicTypeDefinition(
    Type Type,
    List<JsonDerivedType> DerivedTypes,
    string DiscriminatorPropertyName = "$type");
