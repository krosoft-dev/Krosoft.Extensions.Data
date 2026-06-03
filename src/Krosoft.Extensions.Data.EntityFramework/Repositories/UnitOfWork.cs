using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext dbContext)
    {
        _context = dbContext;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public int SaveChanges()
    {
        var result = _context.SaveChanges();
        _context.ChangeTracker.Clear();
        return result;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var result = await _context.SaveChangesAsync(cancellationToken);
        _context.ChangeTracker.Clear();
        return result;
    }
}