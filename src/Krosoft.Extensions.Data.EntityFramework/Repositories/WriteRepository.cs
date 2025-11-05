using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Krosoft.Extensions.Data.EntityFramework.Repositories;

public sealed class WriteRepository<TEntity> : IWriteRepository<TEntity>
    where TEntity : class

{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public WriteRepository(DbContext dbContext)
    {
        Guard.IsNotNull(nameof(dbContext), dbContext);

        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public void Delete(TEntity entity)
    {
        Guard.IsNotNull(nameof(entity), entity);

        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    public void DeleteById(params object[] key)
    {
        var entity = Get(key);
        Delete(entity!);
    }

    public async Task DeleteByIdAsync(params object[] key)
    {
        var entity = await GetAsync(key);
        Delete(entity!);
    }

#if NET7_0_OR_GREATER

    public Task DeleteRangeAsync(CancellationToken cancellationToken) 
        => _dbSet.ExecuteDeleteAsync(cancellationToken);

    public Task DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate,
                                 CancellationToken cancellationToken) 
        => _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);

    public Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate,
                                 Action<IUpdatePropertyBuilder<TEntity>> setProperties,
                                 CancellationToken cancellationToken)
    {
       



        Guard.IsNotNull(nameof(predicate), predicate);
  Guard.IsNotNull(nameof(setProperties), setProperties);




  var builder = new UpdatePropertyBuilder();
  setProperties(builder);
    
  if (!builder.Properties.Any())
      throw new ArgumentException("Au moins une propriété doit être spécifiée");

  Expression<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, 
      Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>> setterExpression 
      = BuildSetterExpression(builder.Properties);
    
//        var builder = new UpdatePropertyBuilder();
//        setProperties(builder);
    
//        var query = _dbSet.Where(predicate);

//        //await context.Blogs
//        //             .Where(b => b.Rating < 3)
//        //             .ExecuteUpdateAsync(setters => setters
//        //                                            .SetProperty(b => b.IsVisible, false)
//        //                                            .SetProperty(b => b.Rating, 0)
//        //                                 , cancellationToken);


        return _dbSet.Where(predicate).ExecuteUpdateAsync(setterExpression ,cancellationToken);
    }
    private static Expression<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, 
                               Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>> 
    BuildSetterExpression(List<(LambdaExpression PropertyExpression, object? Value)> properties)
{
    var settersParam = Expression.Parameter(
        typeof(Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>), 
        "setters");
    
    Expression body = settersParam;
    
    foreach (var (propertyExpression, value) in properties)
    {
        var propertyType = propertyExpression.ReturnType;
        
        // Récupérer toutes les méthodes SetProperty
        var allMethods = typeof(Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>)
            .GetMethods()
            .Where(m => m.Name == "SetProperty" && m.IsGenericMethodDefinition)
            .ToList();
        
        // Prendre la première (il devrait y en avoir une seule génériquement définie)
        var setPropertyMethod = allMethods.FirstOrDefault(m =>
        {
            var parameters = m.GetParameters();
            // Chercher celle avec 3 paramètres (this, Expression<Func<>>, TProperty)
            return parameters.Length == 3 && 
                   parameters[2].ParameterType.IsGenericParameter;
        });
        
        if (setPropertyMethod == null)
            throw new InvalidOperationException($"SetProperty method not found. Available methods: {string.Join(", ", allMethods.Select(m => m.ToString()))}");
        
        setPropertyMethod = setPropertyMethod.MakeGenericMethod(propertyType);
        
        var valueConstant = Expression.Constant(value, propertyType);
        
        body = Expression.Call(
            setPropertyMethod,
            body,
            Expression.Quote(propertyExpression),
            valueConstant);
    }
    
    return Expression.Lambda<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, 
                                  Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>>(
        body, 
        settersParam);
}

private class UpdatePropertyBuilder : IUpdatePropertyBuilder<TEntity>
{
    public List<(LambdaExpression PropertyExpression, object? Value)> Properties { get; } = new();
    
    public IUpdatePropertyBuilder<TEntity> SetProperty<TProperty>(
        Expression<Func<TEntity, TProperty>> property, 
        TProperty value)
    {
        Properties.Add((property, value));
        return this;
    }
}
 






    public Task UpdateRangeAsync( Action<IUpdatePropertyBuilder<TEntity>> setProperties,
                                  CancellationToken cancellationToken)
    {

        
         
        Guard.IsNotNull(nameof(setProperties), setProperties);




        var builder = new UpdatePropertyBuilder();
        setProperties(builder);
    
        if (!builder.Properties.Any())
            throw new ArgumentException("Au moins une propriété doit être spécifiée");

        Expression<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, 
            Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>> setterExpression 
            = BuildSetterExpression(builder.Properties);

        return _dbSet. ExecuteUpdateAsync(setterExpression ,cancellationToken);
    }


//    private class UpdatePropertyBuilder : IUpdatePropertyBuilder<TEntity>
//    {
//        public List<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, 
//            Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>> Updates { get; } = new();
    
//        public IUpdatePropertyBuilder<TEntity> SetProperty<TProperty>(
//            Expression<Func<TEntity, TProperty>> property, 
//            TProperty value)
//        {
//            Updates.Add(setters => setters.SetProperty(property, value));
//            return this;
//        }
//    }



//    public Task<int> ExecuteUpdateAsync(
//        Expression<Func<TEntity, bool>> predicate,
//        Action<IUpdatePropertyBuilder<TEntity>> setProperties,
//        CancellationToken cancellationToken = default)
//    {
//        Guard.IsNotNull(nameof(predicate), predicate);
//        Guard.IsNotNull(nameof(setProperties), setProperties);
    
//        var builder = new UpdatePropertyBuilder();
//        setProperties(builder);
    
//        var query = _dbSet.Where(predicate);

//        //await context.Blogs
//        //             .Where(b => b.Rating < 3)
//        //             .ExecuteUpdateAsync(setters => setters
//        //                                            .SetProperty(b => b.IsVisible, false)
//        //                                            .SetProperty(b => b.Rating, 0)
//        //                                 , cancellationToken);

//        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls = Build(setProperties);
//        return query.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
//    }

//    private static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> 
//        Build(Action<IUpdatePropertyBuilder<TEntity>> setProperties)
//    {
//        foreach (var VARIABLE in null)
//        {
            

//        }
//    }

//    //public static Task<int> ExecuteUpdateAsync<TSource>(
//    //    this IQueryable<TSource> source,
//    //    Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> setPropertyCalls,
//    //    CancellationToken cancellationToken = default)
//    //    => source.Provider is IAsyncQueryProvider provider
//    //        ? provider.ExecuteAsync<Task<int>>(
//    //                                           Expression.Call(
//    //                                                           ExecuteUpdateMethodInfo.MakeGenericMethod(typeof(TSource)), source.Expression, setPropertyCalls), cancellationToken)
//    //        : throw new InvalidOperationException(CoreStrings.IQueryableProviderNotAsync);

//    //internal static readonly MethodInfo ExecuteUpdateMethodInfo
//    //    = typeof(EntityFrameworkQueryableExtensions).GetTypeInfo().GetDeclaredMethod(nameof(ExecuteUpdate))!;


    



//    public Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> predicate,
//        (Expression<Func<TEntity, object?>> property, object? value)[] updates,
//        CancellationToken cancellationToken)
//    {
//        Guard.IsNotNull(nameof(predicate), predicate);
//        Guard.IsNotNull(nameof(updates), updates);
         
//        if (updates.Length == 0)
//            throw new KrosoftTechnicalException("Au moins une propriété doit être spécifiée");
//            var query = _dbSet.Where(predicate);
    
//    return query.ExecuteUpdateAsync(setters =>
//    {
//        foreach (var (property, value) in updates)
//        {
//           Argument 1 : conversion impossible de 'System.Linq.Expressions.Expression<System.Func<TEntity, object?>>' en 'System.Func<TEntity, object>'
//        }
//        return setters;
//    }, cancellationToken);
         
////    }

////    public Task<int> ExecuteUpdateAsync(
////    Expression<Func<TEntity, bool>> predicate,
////    CancellationToken cancellationToken,
////    params (Expression<Func<TEntity, object?>> property, object? value)[] updates)
////{
////    Guard.IsNotNull(nameof(predicate), predicate);
////    Guard.IsNotNull(nameof(updates), updates);
     

//}





#endif
    public void DeleteRange()
    {
        DeleteRange(_dbSet);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        Guard.IsNotNull(nameof(entities), entities);

        _dbSet.RemoveRange(entities);
    }

    public void DeleteRange(Expression<Func<TEntity, bool>> predicate)
    {
        var query = _dbSet.Where(predicate);
        _dbSet.RemoveRange(query);
    }

    public TEntity? Get(params object[] key) => _dbSet.Find(key);

    public ValueTask<TEntity?> GetAsync(params object[] key) => _dbSet.FindAsync(key);

    public void Insert(TEntity entity)
    {
        Guard.IsNotNull(nameof(entity), entity);

        _dbSet.Add(entity);
    }

    public void InsertRange(IEnumerable<TEntity> entities)
    {
        Guard.IsNotNull(nameof(entities), entities);

        _dbSet.AddRange(entities);
    }

    public void InsertUpdateDelete(CrudBusiness<TEntity> crudBusiness)
    {
        InsertRange(crudBusiness.ToAdd);
        UpdateRange(crudBusiness.ToUpdate);
        DeleteRange(crudBusiness.ToDelete);
    }

    public IQueryable<TEntity> Query() => _dbSet;

    public void Update(TEntity entityToUpdate)
    {
        Guard.IsNotNull(nameof(entityToUpdate), entityToUpdate);

        _dbSet.Update(entityToUpdate);
    }

    public void Update(TEntity entityToUpdate, params Expression<Func<TEntity, object?>>[] propertiesExpression)
    {
        Guard.IsNotNull(nameof(entityToUpdate), entityToUpdate);
        _dbSet.Attach(entityToUpdate);

        foreach (var propertyExpression in propertiesExpression)
        {
            _dbContext.Entry(entityToUpdate).Property(propertyExpression).IsModified = true;
        }
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        Guard.IsNotNull(nameof(entities), entities);
        _dbSet.UpdateRange(entities);
    }

    public void UpdateRange(IEnumerable<TEntity> entities,
                            params Expression<Func<TEntity, object?>>[] propertiesExpression)
    {
        foreach (var entity in entities)
        {
            Update(entity, propertiesExpression);
        }
    }
}