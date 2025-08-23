using Krosoft.Extensions.Data.EntityFramework.Contexts;

namespace Krosoft.Extensions.Data.EntityFramework.Models;

public interface ITenantAuditableDbContextSettings<T> : IDbContextSettings<T> where T : KrosoftContext
{
    string TenantId { get; }
    string UserId { get; }
    DateTimeOffset Now { get; }
}