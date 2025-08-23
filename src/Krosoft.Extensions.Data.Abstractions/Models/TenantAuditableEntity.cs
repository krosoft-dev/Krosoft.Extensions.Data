namespace Krosoft.Extensions.Data.Abstractions.Models;

public abstract record TenantAuditableEntity<T> : AuditableEntity, ITenant<T> where T : class
{
    public T? TenantId { get; set; }
}