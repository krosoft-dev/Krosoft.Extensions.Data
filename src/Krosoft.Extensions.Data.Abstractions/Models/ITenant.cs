namespace Krosoft.Extensions.Data.Abstractions.Models;

public interface ITenant<T>
{
    public T? TenantId { get; set; }
}