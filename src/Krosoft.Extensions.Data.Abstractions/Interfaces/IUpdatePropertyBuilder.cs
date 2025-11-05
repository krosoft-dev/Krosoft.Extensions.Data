using System.Linq.Expressions;

namespace Krosoft.Extensions.Data.Abstractions.Interfaces;

public interface IUpdatePropertyBuilder<TEntity>
{
    IUpdatePropertyBuilder<TEntity> SetProperty<TProperty>(Expression<Func<TEntity, TProperty>> property,
                                                           TProperty value);
}