using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Models;

[TestClass]
public class TenantAuditableEntityTests
{
    [TestMethod]
    public void TenantAuditableEntity_DefaultConstructor()
    {
        var entity = new Utilisateur();

        Check.That(entity.CreatedAt).IsEqualTo(DateTimeOffset.MinValue);
        Check.That(entity.UpdatedAt).IsEqualTo(DateTimeOffset.MinValue);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetCreateurDate()
    {
        var entity = new Utilisateur();
        var createdAt = DateTimeOffset.Now.AddDays(-1);

        entity.CreatedAt = createdAt;

        Check.That(entity.CreatedAt).IsEqualTo(createdAt);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetCreateurId()
    {
        var entity = new Utilisateur();
        var createdBy = "user123";

        entity.CreatedBy = createdBy;

        Check.That(entity.CreatedBy).IsEqualTo(createdBy);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetUpdatedAt()
    {
        var entity = new Utilisateur();
        var updatedAt = DateTimeOffset.Now.AddDays(-1);

        entity.UpdatedAt = updatedAt;

        Check.That(entity.UpdatedAt).IsEqualTo(updatedAt);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetModificateurId()
    {
        var entity = new Utilisateur();
        var updatedBy = "user456";

        entity.UpdatedBy = updatedBy;

        Check.That(entity.UpdatedBy).IsEqualTo(updatedBy);
    }

    [TestMethod]
    public void TenantAuditableEntity_SetTenantId()
    {
        var entity = new Utilisateur();
        var tenantId = "user456";

        entity.TenantId = tenantId;

        Check.That(entity.TenantId).IsEqualTo(tenantId);
    }
}