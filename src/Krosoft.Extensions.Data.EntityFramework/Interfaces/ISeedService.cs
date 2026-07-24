using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Interfaces;

public interface ISeedService<TDbContext> where TDbContext : DbContext
{
    bool Initialized { get; set; }
    void InitializeDbForTests(TDbContext db);
}


