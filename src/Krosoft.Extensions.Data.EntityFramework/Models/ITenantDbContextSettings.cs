using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public interface ITenantDbContextSettings<T, out TTenantId> : IDbContextSettings<T> where T : KrosoftContext
{
    TTenantId TenantId { get; }
}