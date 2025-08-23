namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface ITenantDbContextProvider<out T>
{
    T GetTenantId();
}