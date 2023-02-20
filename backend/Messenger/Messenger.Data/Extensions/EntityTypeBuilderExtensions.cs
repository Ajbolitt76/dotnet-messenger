using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Data.Extensions;

/// <summary>
/// Расширения <see cref="EntityTypeBuilder"/>
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Установить доступ EF к navigation property по полю
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <param name="builder">Строитель</param>
    /// <param name="navigation">Navigation property</param>
    /// <param name="fieldName">Название поля</param>
    /// <returns>Строитель</returns>
    public static EntityTypeBuilder<T> SetPropertyAccessModeField<T>(
        this EntityTypeBuilder<T> builder,
        Expression<Func<T, object?>> navigation,
        string fieldName)
        where T : class
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (navigation is null)
            throw new ArgumentNullException(nameof(navigation));
        if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentException($"'{nameof(fieldName)}' cannot be null or empty.", nameof(fieldName));

        builder.Property(navigation)
            .HasField(fieldName)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        return builder;
    }

}