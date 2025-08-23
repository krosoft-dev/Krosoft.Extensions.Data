using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.Abstractions.Helpers;
using LinqKit;
using System.Linq.Expressions;
using System.Reflection;

namespace Krosoft.Extensions.Data.Abstractions.Extensions;

public static class QueryableExtensions
{
    private static readonly MethodInfo? ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
    private static readonly MethodInfo? ToLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

    public static IQueryable<T> Filter<T>(this IQueryable<T> query,
                                          params ExpressionStarter<T>[] predicates)
    {
        var predicate = PredicateBuilder.New<T>();
        foreach (var item in predicates)
        {
            predicate = predicate.Or(item);
        }

        query = query.Where(predicate);

        return query;
    }

    public static IQueryable<T> Filter<T, TItem>(this IQueryable<T> query,
                                                 IEnumerable<TItem> items,
                                                 Func<TItem, Expression<Func<T, bool>>> func,
                                                 bool isMandatory = false)
    {
        Guard.IsNotNull(nameof(items), items);
        Guard.IsNotNull(nameof(func), func);

        var uniqueItems = items.ToHashSet();
        if (!isMandatory && uniqueItems.Count <= 0)
        {
            return query;
        }

        var predicate = PredicateHelper.Or(uniqueItems, func);

        query = query.Where(predicate);

        return query;
    }

    public static IQueryable<T> Filter<T, TItem>(this IQueryable<T> query,
                                                 TItem? item,
                                                 Func<TItem?, Expression<Func<T, bool>>> getLambdaExpression)
        where TItem : struct
    {
        if (item.HasValue)
        {
            query = query.Where(getLambdaExpression(item));
        }

        return query;
    }

    public static IQueryable<T> Search<T>(this IQueryable<T> query,
                                          string? searchTerm,
                                          params Expression<Func<T, string?>>[] selectors)
    {
        if (ToLowerMethod == null)
        {
            throw new KrosoftTechnicalException("Method 'ToLower' is not defined.");
        }

        if (ContainsMethod == null)
        {
            throw new KrosoftTechnicalException("Method 'Contains' is not defined.");
        }

        if (string.IsNullOrEmpty(searchTerm))
        {
            return query;
        }

        Expression right = Expression.Constant(searchTerm);
        right = Expression.Call(right, ToLowerMethod);

        var predicate = PredicateBuilder.New<T>();
        foreach (var selector in selectors)
        {
            var left = selector.Body;
            left = Expression.Call(left, ToLowerMethod);
            var containsBound = Expression.Call(left, ContainsMethod, right);
            var lambda = Expression.Lambda<Func<T, bool>>(containsBound, selector.Parameters);

            predicate = predicate.Or(lambda);
        }

        return query.Where(predicate);
    }

    public static IQueryable<T> SearchAll<T>(this IQueryable<T> query,
                                             string? searchTerm,
                                             Expression<Func<T, string?>> selector)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return query;
        }

        var parts = searchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 0)
        {
            foreach (var part in parts)
            {
                query = query.Search(part, selector);
            }
        }

        return query;
    }

    public static IQueryable<T> SortBy<T>(this IQueryable<T> query,
                                          IPaginationRequest request)
    {
        if (request.SortBy != null)
        {
            var sort = request.SortBy.ToArray();

            if (sort.Length > 0)
            {
                foreach (var sortOption in sort)
                {
                    var parts = sortOption.Split(':');
                    if (parts.Length == 2)
                    {
                        var key = parts[0];
                        var order = parts[1].ToLower();
                        var prop = typeof(T).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (prop is null)
                        {
                            throw new KrosoftTechnicalException($"Impossible de déterminer la colonne à partir de la clé suivante : {key}");
                        }

                        // Créer une expression pour le tri.
                        var parameter = Expression.Parameter(typeof(T), "x");
                        var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);
                        // Appliquer le tri.
                        var methodName = order == "asc" ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);
                        var resultExp = Expression.Call(typeof(Queryable), methodName, new[] { typeof(T), prop.PropertyType }, query.Expression, orderByExp);
                        query = query.Provider.CreateQuery<T>(resultExp);
                    }
                }
            }
        }

        return query;
    }
}