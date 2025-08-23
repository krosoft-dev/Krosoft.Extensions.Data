namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface IAuditableDbContextProvider
{
    DateTimeOffset GetNow();
    string GetUserId();
}