using Krosoft.Extensions.Data.Abstractions.Interfaces;
#if NET7_0_OR_GREATER
using Microsoft.EntityFrameworkCore;
#endif

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public class BatchDbService : IBatchDbService
{
    public Task<int> ExecuteDeleteAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken)
    {
#if NET7_0_OR_GREATER
        return query.ExecuteDeleteAsync(cancellationToken);
#else
        throw new NotSupportedException("ExecuteDeleteAsync requires EF Core 7+.");
#endif
    }
}