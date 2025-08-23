#if NET8_0_OR_GREATER
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<T> HasJson<T>(this PropertyBuilder<T> propertyBuilder)
        where T : class, new()
    {
        var converter = new ValueConverter<T, string>
            (
             v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
             v => JsonSerializer.Deserialize<T>(v, JsonSerializerOptions.Default) ?? new T()
            );

        var comparer = new ValueComparer<T>
            (
             (l, r) => JsonSerializer.Serialize(l, JsonSerializerOptions.Default) ==
                       JsonSerializer.Serialize(r, JsonSerializerOptions.Default),
             v => (T?)v == null ? 0 : JsonSerializer.Serialize(v, JsonSerializerOptions.Default).GetHashCode(),
             v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                                                JsonSerializerOptions.Default)!
            );

        propertyBuilder.HasConversion(converter);
        propertyBuilder.Metadata.SetValueConverter(converter);
        propertyBuilder.Metadata.SetValueComparer(comparer);
        propertyBuilder.HasColumnType("jsonb");

        return propertyBuilder;
    }
}

#endif