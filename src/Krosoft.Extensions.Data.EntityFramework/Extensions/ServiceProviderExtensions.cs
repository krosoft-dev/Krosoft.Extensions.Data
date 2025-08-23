//using Krosoft.Extensions.Data.EntityFramework.Contexts;
//using Krosoft.Extensions.Data.EntityFramework.Interfaces;
//using Krosoft.Extensions.Data.EntityFramework.Models;
//using Krosoft.Extensions.Data.EntityFramework.Scopes;
//using Microsoft.Extensions.DependencyInjection;

//namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

//public static class ServiceProviderExtensions
//{
//    public static IDbContextScope CreateDbContextScope<T, TTenantId>(this IServiceProvider provider,
//                                                                     TTenantId tenantId,
//                                                                     DateTime now,
//                                                                     string userId)
//        where T : KrosoftTenantAuditableContext<TTenantId>
//        => CreateDbContextScope(provider,
//                                new TenantAuditableDbContextSettings<T, TTenantId>(tenantId,
//                                                                                   now,
//                                                                                   userId));

//    public static IDbContextScope CreateDbContextScope<T>(this IServiceProvider provider,
//                                                          IDbContextSettings<T> dbContextSettings)
//        where T : KrosoftContext
//        => new DbContextScope<T>(provider.CreateScope(),
//                                 dbContextSettings);

//    public static IReadDbContextScope CreateReadDbContextScope<T>(this IServiceProvider provider,
//                                                                  IDbContextSettings<T> dbContextSettings)
//        where T : KrosoftContext
//        => new ReadDbContextScope<T>(provider.CreateScope(),
//                                     dbContextSettings);
//}

using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Data.EntityFramework.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Crée un scope de contexte tenant + audit (votre méthode actuelle)
    /// </summary>
    public static IDbContextScope CreateDbContextScope<T, TTenantId>(this IServiceProvider provider,
                                                                     TTenantId tenantId,
                                                                     DateTime now,
                                                                     string userId)
        where T : KrosoftTenantAuditableContext<TTenantId>
        => CreateDbContextScope(provider,
                                new TenantAuditableDbContextSettings<T, TTenantId>(tenantId, now, userId));

    /// <summary>
    /// Crée un scope de contexte tenant uniquement
    /// </summary>
    public static IDbContextScope CreateTenantDbContextScope<T, TTenantId>(this IServiceProvider provider,
                                                                           TTenantId tenantId)
        where T : KrosoftTenantContext<TTenantId>
        => CreateDbContextScope(provider,
                                new TenantDbContextSettings<T, TTenantId>(tenantId));

    /// <summary>
    /// Crée un scope de contexte audit uniquement
    /// </summary>
    public static IDbContextScope CreateAuditableDbContextScope<T>(this IServiceProvider provider,
                                                                   DateTime now,
                                                                   string userId)
        where T : KrosoftAuditableContext
        => CreateDbContextScope(provider,
                                new AuditableDbContextSettings<T>(now, userId));

    /// <summary>
    /// Crée un scope de contexte basique (sans tenant ni audit)
    /// </summary>
    public static IDbContextScope CreateBasicDbContextScope<T>(this IServiceProvider provider)
        where T : KrosoftContext
        => CreateDbContextScope(provider,
                                new DbContextSettings<T>());

    /// <summary>
    /// Crée un scope de contexte avec settings personnalisés
    /// </summary>
    public static IDbContextScope CreateDbContextScope<T>(this IServiceProvider provider,
                                                          IDbContextSettings<T> dbContextSettings)
        where T : KrosoftContext
        => new DbContextScope<T>(provider.CreateScope(), dbContextSettings);

    /// <summary>
    /// Crée un scope de contexte en lecture seule avec settings personnalisés
    /// </summary>
    public static IReadDbContextScope CreateReadDbContextScope<T>(this IServiceProvider provider,
                                                                  IDbContextSettings<T> dbContextSettings)
        where T : KrosoftContext
        => new ReadDbContextScope<T>(provider.CreateScope(), dbContextSettings);

    /// <summary>
    /// Crée un scope de lecture tenant + audit
    /// </summary>
    public static IReadDbContextScope CreateReadDbContextScope<T, TTenantId>(this IServiceProvider provider,
                                                                             TTenantId tenantId,
                                                                             DateTime now,
                                                                             string userId)
        where T : KrosoftTenantAuditableContext<TTenantId>
        => CreateReadDbContextScope(provider,
                                    new TenantAuditableDbContextSettings<T, TTenantId>(tenantId, now, userId));

    /// <summary>
    /// Crée un scope de lecture tenant uniquement
    /// </summary>
    public static IReadDbContextScope CreateTenantReadDbContextScope<T, TTenantId>(this IServiceProvider provider,
                                                                                   TTenantId tenantId)
        where T : KrosoftTenantContext<TTenantId>
        => CreateReadDbContextScope(provider,
                                    new TenantDbContextSettings<T, TTenantId>(tenantId));

    /// <summary>
    /// Crée un scope de lecture audit uniquement
    /// </summary>
    public static IReadDbContextScope CreateAuditableReadDbContextScope<T>(this IServiceProvider provider,
                                                                           DateTime now,
                                                                           string userId)
        where T : KrosoftAuditableContext
        => CreateReadDbContextScope(provider,
                                    new AuditableDbContextSettings<T>(now, userId));

    /// <summary>
    /// Crée un scope de lecture basique
    /// </summary>
    public static IReadDbContextScope CreateBasicReadDbContextScope<T>(this IServiceProvider provider)
        where T : KrosoftContext
        => CreateReadDbContextScope(provider,
                                    new DbContextSettings<T>());

    /// <summary>
    /// Détecte automatiquement le type de contexte et crée le scope approprié
    /// </summary>
    //public static IDbContextScope CreateAutoDbContextScope<T>(this IServiceProvider provider,
    //                                                          object? tenantId = null,
    //                                                          DateTime? now = null,
    //                                                          string? userId = null)
    //    where T : KrosoftContext
    //{
    //    var contextType = typeof(T);
    //    var hasAudit = IsAuditableContext(contextType);
    //    var hasTenant = IsTenantContext(contextType);

    //    return (hasAudit, hasTenant) switch
    //    {
    //        (true, true) when tenantId != null && now != null => 
    //            CreateDbContextScope(provider, CreateTenantAuditableSettings<T>(tenantId, now.Value, userId)),

    //        (true, false) when now != null => 
    //            CreateAuditableDbContextScope<T>(provider, now.Value, userId ?? string.Empty),

    //        (false, true) when tenantId != null => 
    //            CreateDbContextScope(provider, CreateTenantSettings<T>(tenantId)),

    //        (false, false) => 
    //            CreateBasicDbContextScope<T>(provider),

    //        _ => throw new InvalidOperationException(
    //            $"Impossible de créer un contexte pour {typeof(T).Name} avec les paramètres fournis. " +
    //            $"Audit: {hasAudit}, Tenant: {hasTenant}, TenantId: {tenantId}, Now: {now}")
    //    };
    //}
    private static bool IsAuditableContext(Type contextType) =>
        contextType.BaseType?.IsGenericType == true &&
        (contextType.BaseType.GetGenericTypeDefinition() == typeof(KrosoftAuditableContext) ||
         contextType.BaseType.GetGenericTypeDefinition() == typeof(KrosoftTenantAuditableContext<>) ||
         (contextType.BaseType.GetGenericTypeDefinition() == typeof(KrosoftConfigurableContext<>) &&
          HasAuditableConstructor(contextType)));

    private static bool IsTenantContext(Type contextType) =>
        contextType.BaseType?.IsGenericType == true &&
        (contextType.BaseType.GetGenericTypeDefinition() == typeof(KrosoftTenantContext<>) ||
         contextType.BaseType.GetGenericTypeDefinition() == typeof(KrosoftTenantAuditableContext<>) ||
         (contextType.BaseType.GetGenericTypeDefinition() == typeof(KrosoftConfigurableContext<>) &&
          HasTenantConstructor(contextType)));

    private static bool HasAuditableConstructor(Type contextType)
    {
        return contextType.GetConstructors()
                          .Any(c => c.GetParameters()
                                     .Any(p => p.ParameterType == typeof(IAuditableDbContextProvider)));
    }

    private static bool HasTenantConstructor(Type contextType)
    {
        return contextType.GetConstructors()
                          .Any(c => c.GetParameters()
                                     .Any(p => p.ParameterType.IsGenericType &&
                                               p.ParameterType.GetGenericTypeDefinition() == typeof(ITenantDbContextProvider<>)));
    }

    private static IDbContextSettings<T> CreateTenantAuditableSettings<T>(object tenantId, DateTime now, string? userId)
        where T : KrosoftContext
    {
        var settingsType = typeof(TenantAuditableDbContextSettings<,>).MakeGenericType(typeof(T), tenantId.GetType());
        return (IDbContextSettings<T>)Activator.CreateInstance(settingsType, tenantId, now, userId)!;
    }

    private static IDbContextSettings<T> CreateTenantSettings<T>(object tenantId)
        where T : KrosoftContext
    {
        var settingsType = typeof(TenantDbContextSettings<,>).MakeGenericType(typeof(T), tenantId.GetType());
        return (IDbContextSettings<T>)Activator.CreateInstance(settingsType, tenantId)!;
    }

    public static IDbContextScope CreateDbContextScope<T, TTenantId>(this IServiceProvider provider,
                                                                     TTenantId tenantId,
                                                                     DateTimeOffset now,
                                                                     string userId)  
          where T : KrosoftTenantAuditableContext<TTenantId>
         => CreateDbContextScope(provider,
                                 new TenantAuditableDbContextSettings<T, TTenantId>(tenantId,
                                                                                    now,
                                                                                    userId));
}