using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public record TenantDbContextSettings<T, TTenantId> : ITenantDbContextSettings<T, TTenantId>
    where T : KrosoftTenantContext<TTenantId>
{
    public TenantDbContextSettings(TTenantId tenantId)
    {
        TenantId = tenantId;
    }

    public TTenantId TenantId { get; }
}