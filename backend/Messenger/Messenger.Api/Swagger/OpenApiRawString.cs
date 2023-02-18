using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Writers;

namespace Messenger.Api.Swagger;

public class OpenApiRawString : IOpenApiAny, IOpenApiPrimitive
{
    public OpenApiRawString(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
    {
        writer.WriteRaw(Value);
    }

    /// <inheritdoc/>
    public AnyType AnyType => AnyType.Primitive;

    /// <inheritdoc/>
    public PrimitiveType PrimitiveType => PrimitiveType.String;
    
    public static OpenApiString ToOpenApiString(OpenApiRawString value)
        => new(value.Value);
}
