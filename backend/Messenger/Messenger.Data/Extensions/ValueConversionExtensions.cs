using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Messenger.Data.Extensions;

public static class ValueConversionExtensions
{
    /// <summary>
    /// Установить тип и преобразование
    /// </summary>
    /// <typeparam name="T">Десериализованный тип</typeparam>
    /// <typeparam name="TDeserialize">Тип в который десериализуют</typeparam>
    /// <param name="propertyBuilder">Строитель</param>
    /// <param name="options">Опции json</param>
    /// <returns>Строитель</returns>
    public static PropertyBuilder<T> HasJsonConversion<T,TDeserialize>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions? options = null)
        where TDeserialize : class, T
    {
        options ??= new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

            Converters =
            {
                new JsonStringEnumConverter(),
            },
        };

        var converter = new ValueConverter<T, string>(
            v => JsonSerializer.Serialize<T>(v, options),
            v => (T)JsonSerializer.Deserialize<TDeserialize>(v, options)!);

        propertyBuilder.HasConversion(converter);
        propertyBuilder.HasColumnType("jsonb");

        return propertyBuilder;
    }

    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, JsonSerializerOptions? options = null)
        where T : class?
        => HasJsonConversion<T, T>(propertyBuilder, options);
}