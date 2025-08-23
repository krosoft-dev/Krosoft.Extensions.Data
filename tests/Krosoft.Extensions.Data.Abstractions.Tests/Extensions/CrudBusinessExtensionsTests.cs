using Krosoft.Extensions.Data.Abstractions.Extensions;
using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Extensions;

[TestClass]
public class CrudBusinessExtensionsTests
{
    [TestMethod]
    public void Any_With_All()
    {
        var toBdd = new CrudBusiness<Item>();

        toBdd.ToDelete.Add(new Item());
        toBdd.ToAdd.Add(new Item());
        toBdd.ToUpdate.Add(new Item());

        Check.That(toBdd.ToDelete).HasSize(1);
        Check.That(toBdd.ToAdd).HasSize(1);
        Check.That(toBdd.ToUpdate).HasSize(1);
        Check.That(toBdd.Any()).IsTrue();
    }

    [TestMethod]
    public void Any_With_Add()
    {
        var toBdd = new CrudBusiness<Item>();

        toBdd.ToAdd.Add(new Item());

        Check.That(toBdd.ToDelete).IsEmpty();
        Check.That(toBdd.ToAdd).HasSize(1);
        Check.That(toBdd.ToUpdate).IsEmpty();
        Check.That(toBdd.Any()).IsTrue();
    }

    [TestMethod]
    public void Any_With_Delete()
    {
        var toBdd = new CrudBusiness<Item>();

        toBdd.ToDelete.Add(new Item());

        Check.That(toBdd.ToDelete).HasSize(1);
        Check.That(toBdd.ToAdd).IsEmpty();
        Check.That(toBdd.ToUpdate).IsEmpty();
        Check.That(toBdd.Any()).IsTrue();
    }

    [TestMethod]
    public void Any_With_Update()
    {
        var toBdd = new CrudBusiness<Item>();

        toBdd.ToUpdate.Add(new Item());

        Check.That(toBdd.ToDelete).IsEmpty();
        Check.That(toBdd.ToAdd).IsEmpty();
        Check.That(toBdd.ToUpdate).HasSize(1);
        Check.That(toBdd.Any()).IsTrue();
    }

    [TestMethod]
    public void Any_Empty()
    {
        var toBdd = new CrudBusiness<Item>();

        Check.That(toBdd.ToDelete).IsEmpty();
        Check.That(toBdd.ToAdd).IsEmpty();
        Check.That(toBdd.ToUpdate).IsEmpty();
        Check.That(toBdd.Any()).IsFalse();
    }
}