using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Models;

[TestClass]
public class AuditableEntityTests
{
    [TestMethod]
    public void AuditableEntity_DefaultConstructor()
    {
        var entity = new Pays();

        Check.That(entity.CreatedAt).IsEqualTo(DateTimeOffset.MinValue);
        Check.That(entity.UpdatedAt).IsEqualTo(DateTimeOffset.MinValue);
    }

    [TestMethod]
    public void AuditableEntity_SetCreatedAt()
    {
        var entity = new Pays();
        var createurDate = DateTimeOffset.Now.AddDays(-1);

        entity.CreatedAt = createurDate;

        Check.That(entity.CreatedAt).IsEqualTo(createurDate);
    }

    [TestMethod]
    public void AuditableEntity_SetCreatedBy()
    {
        var entity = new Pays();
        var createurId = "user123";

        entity.CreatedBy = createurId;

        Check.That(entity.CreatedBy).IsEqualTo(createurId);
    }

    [TestMethod]
    public void AuditableEntity_SetUpdatedAt()
    {
        var entity = new Pays();
        var updatedAt = DateTimeOffset.Now.AddDays(-1);

        entity.UpdatedAt = updatedAt;

        Check.That(entity.UpdatedAt).IsEqualTo(updatedAt);
    }

    [TestMethod]
    public void AuditableEntity_SetUpdatedBy()
    {
        var entity = new Pays();
        var modificateurId = "user456";

        entity.UpdatedBy = modificateurId;

        Check.That(entity.UpdatedBy).IsEqualTo(modificateurId);
    }
}