using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class AuditableDbContextProvider : IAuditableDbContextProvider
{
    private readonly DateTimeOffset _now;
    private readonly string _userId;

    public AuditableDbContextProvider()
    {
        _now = DateTime.MinValue;
        _userId = string.Empty;
    }

    public AuditableDbContextProvider(DateTimeOffset now, string userId)
    {
        _now = now;
        _userId = userId;
    }

    public DateTimeOffset GetNow() => _now;

    public string GetUserId() => _userId;
}