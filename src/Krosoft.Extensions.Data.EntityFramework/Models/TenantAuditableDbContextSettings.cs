using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public record TenantAuditableDbContextSettings<T, TTenantId> : ITenantAuditableDbContextSettings<T, TTenantId>
    where T : KrosoftTenantAuditableContext<TTenantId>
{
    public TenantAuditableDbContextSettings(TTenantId tenantId, DateTimeOffset now, string userId)
    {
        Now = now;
        UserId = userId;
        TenantId = tenantId;
    }

    public DateTimeOffset Now { get; }

    public TTenantId TenantId { get; }

    public string UserId { get; }
}