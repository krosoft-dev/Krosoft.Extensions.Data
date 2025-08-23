using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class TenantDbContextProvider<T> : ITenantDbContextProvider<T>
{
    private readonly T _tenantId;

    public TenantDbContextProvider(T tenantId)
    {
        _tenantId = tenantId;
    }

    public T GetTenantId() => _tenantId;
}