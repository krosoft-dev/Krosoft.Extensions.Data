using System.Linq.Expressions;
using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IWriteRepository<TEntity> : IDisposable where TEntity : class
{
    void Delete(TEntity entity);

    void DeleteById(params object[] key);

    Task DeleteByIdAsync(params object[] key);

    void DeleteRange(IEnumerable<TEntity> entities);

    void DeleteRange(Expression<Func<TEntity, bool>> predicate);
    void DeleteRange();
    TEntity? Get(params object[] key);

    ValueTask<TEntity?> GetAsync(params object[] key);

    void Insert(TEntity entity);

    void InsertRange(IEnumerable<TEntity> entities);

    void InsertUpdateDelete(CrudBusiness<TEntity> crudBusiness);
    IQueryable<TEntity> Query();

    void Update(TEntity entityToUpdate);

    void Update(TEntity entityToUpdate,
                params Expression<Func<TEntity, object?>>[] propertiesExpression);

    void UpdateRange(IEnumerable<TEntity> entities);

    void UpdateRange(IEnumerable<TEntity> entities,
                     params Expression<Func<TEntity, object?>>[] propertiesExpression);

#if NET7_0_OR_GREATER
    Task DeleteRangeAsync(CancellationToken cancellationToken);
    Task DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate,
                          CancellationToken cancellationToken);

    Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate,
                               Action<IUpdatePropertyBuilder<TEntity>> setProperties,
                               CancellationToken cancellationToken);
    Task UpdateRangeAsync( Action<IUpdatePropertyBuilder<TEntity>> setProperties,
                           CancellationToken cancellationToken);
#endif
}