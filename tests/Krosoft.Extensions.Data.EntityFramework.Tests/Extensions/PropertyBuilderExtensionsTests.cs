using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Extensions;

[TestClass]
public class PropertyBuilderExtensionsTests
{
    /// <summary>
    /// Construit une propriété <c>IEnumerable&lt;string&gt;</c> configurée via <see cref="PropertyBuilderExtensions.HasJsonCollection{TElement}" />.
    /// </summary>
    private static IMutableProperty GetErreursProperty()
    {
        var modelBuilder = new ModelBuilder();
        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Depot",
                                                                   b => b.IndexerProperty<IEnumerable<string>>("Erreurs")
                                                                         .HasJsonCollection());

        return modelBuilder.Model.FindEntityType("Depot")!.FindProperty("Erreurs")!;
    }

    [TestMethod]
    public void HasJsonCollection_ColumnType()
    {
        var property = GetErreursProperty();

        Check.That(property.GetColumnType()).IsEqualTo("jsonb");
    }

    [TestMethod]
    public void HasJsonCollection_Conversion()
    {
        var converter = GetErreursProperty().GetValueConverter()!;

        var json = converter.ConvertToProvider(new[] { "CIF invalide", "Hub indisponible" });
        var erreurs = (IEnumerable<string>)converter.ConvertFromProvider("[\"CIF invalide\"]")!;
        var vide = (IEnumerable<string>)converter.ConvertFromProvider("null")!;

        Check.That(json).IsEqualTo("[\"CIF invalide\",\"Hub indisponible\"]");
        Check.That(erreurs).ContainsExactly("CIF invalide");
        Check.That(vide).IsEmpty();
    }

    [TestMethod]
    public void HasJsonCollection_Comparer()
    {
        var comparer = GetErreursProperty().GetValueComparer()!;
        var erreurs = new List<string> { "CIF invalide" };

        var snapshot = (IEnumerable<string>)comparer.Snapshot(erreurs)!;
        erreurs.Add("Hub indisponible");

        Check.That(comparer.Equals(new[] { "CIF invalide" }, new List<string> { "CIF invalide" })).IsTrue();
        Check.That(comparer.Equals(snapshot, erreurs)).IsFalse();
        Check.That(snapshot).ContainsExactly("CIF invalide");
    }
}
