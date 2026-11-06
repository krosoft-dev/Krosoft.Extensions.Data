using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<T> HasJson<T>(this PropertyBuilder<T> propertyBuilder)
        where T : class?, new()
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

        return propertyBuilder.HasJsonColumn(converter, comparer);
    }

    /// <summary>
    /// Stocke une collection en colonne <c>jsonb</c> (collection vide si la valeur en base est nulle).
    /// Contrairement à <see cref="HasJson{T}" />, accepte une propriété typée en interface (<c>IEnumerable&lt;T&gt;</c>).
    /// </summary>
    /// <remarks>
    /// Typer la propriété en <c>IEnumerable&lt;T&gt;</c> plutôt qu'en <c>List&lt;T&gt;</c> évite qu'une projection
    /// (<c>ProjectTo</c> vers un DTO de même type) parcoure la collection élément par élément, ce que Npgsql traduit par un
    /// cast <c>jsonb</c> → tableau (« cannot cast type jsonb to text[] »). Si les éléments doivent être mappés vers un autre
    /// type, la projection parcourt toujours la collection : charger alors la colonne à part.
    /// </remarks>
    public static PropertyBuilder<IEnumerable<TElement>> HasJsonCollection<TElement>(this PropertyBuilder<IEnumerable<TElement>> propertyBuilder)
    {
        var converter = new ValueConverter<IEnumerable<TElement>, string>
            (
             v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
             v => JsonSerializer.Deserialize<List<TElement>>(v, JsonSerializerOptions.Default) ?? new List<TElement>()
            );

        var comparer = new ValueComparer<IEnumerable<TElement>>
            (
             (l, r) => l == null ? r == null : r != null && l.SequenceEqual(r),
             v => v == null ? 0 : v.Aggregate(0, (hash, element) => HashCode.Combine(hash, element)),
             v => v.ToList()
            );

        return propertyBuilder.HasJsonColumn(converter, comparer);
    }

    /// <summary>
    /// Applique la conversion JSON et le comparer à la propriété, stockée en colonne <c>jsonb</c>.
    /// </summary>
    private static PropertyBuilder<T> HasJsonColumn<T>(this PropertyBuilder<T> propertyBuilder,
                                                       ValueConverter<T, string> converter,
                                                       ValueComparer<T> comparer) =>
        propertyBuilder.HasConversion(converter, comparer)
                       .HasColumnType("jsonb");
}