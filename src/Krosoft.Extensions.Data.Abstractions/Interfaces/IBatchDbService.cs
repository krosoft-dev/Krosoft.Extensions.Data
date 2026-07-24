namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IBatchDbService
{
    Task<int> ExecuteDeleteAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken);
}