using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Messenger.Core;

namespace Messenger.Infrastructure.Json;

public class PolymorphismJsonOptions
{
    private Dictionary<Type, BasePolymorphicTypeDefinition> _definitions = new();

    public IReadOnlyDictionary<Type, BasePolymorphicTypeDefinition> GetDefinitions() => _definitions;

    public JsonPolymorphismOptions? GetDefinition(Type type)
    {
        if (!_definitions.TryGetValue(type, out var definition))
            return null;

        var options = new JsonPolymorphismOptions
        {
            IgnoreUnrecognizedTypeDiscriminators = true,
            TypeDiscriminatorPropertyName = definition.DiscriminatorPropertyName,
            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
        };

        foreach (var definitionDerivedType in definition.DerivedTypes)
            options.DerivedTypes.Add(definitionDerivedType);

        return options;
    }

    public void AddTypeDefinition(
        Type baseType,
        IEnumerable<JsonDerivedType> typeMapping)
    {
        var oldDefinition = _definitions.GetValueOrDefault(baseType);

        if (oldDefinition is null)
            _definitions[baseType] = new BasePolymorphicTypeDefinition(baseType, typeMapping.ToList());
        else
            oldDefinition.DerivedTypes.AddRange(typeMapping);
    }

    public void AddTypeDefinition<TBase, TDerived>() where TDerived : IHaveJsonDiscriminator
    {
        var baseType = typeof(TBase);
        var oldDefinition = _definitions.GetValueOrDefault(baseType);
        var jsonDefinition = new JsonDerivedType(typeof(TDerived), TDerived.Discriminator);

        if (oldDefinition is null)
            _definitions[baseType] = new BasePolymorphicTypeDefinition(baseType, new() { jsonDefinition });
        else
            oldDefinition.DerivedTypes.Add(jsonDefinition);
    }

    public void AddTypeDefinition<TBase>(
        IEnumerable<JsonDerivedType> typeMapping)
        => AddTypeDefinition(typeof(TBase), typeMapping);
}
