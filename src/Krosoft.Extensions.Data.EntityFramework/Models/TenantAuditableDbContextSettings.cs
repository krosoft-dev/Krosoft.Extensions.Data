using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public record TenantAuditableDbContextSettings<T> : ITenantAuditableDbContextSettings<T>
    where T : KrosoftTenantAuditableContext
{
    public TenantAuditableDbContextSettings(string tenantId, DateTimeOffset now, string userId)
    {
        Now = now;
        UserId = userId;
        TenantId = tenantId;
    }

    public DateTimeOffset Now { get; }

    public string TenantId { get; }

    public string UserId { get; }
}