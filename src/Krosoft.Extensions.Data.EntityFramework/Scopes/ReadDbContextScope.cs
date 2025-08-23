//using Krosoft.Extensions.Core.Models.Exceptions;
//using Krosoft.Extensions.Core.Tools;
//using Krosoft.Extensions.Data.Abstractions.Interfaces;
//using Krosoft.Extensions.Data.EntityFramework.Contexts;
//using Krosoft.Extensions.Data.EntityFramework.Interfaces;
//using Krosoft.Extensions.Data.EntityFramework.Models;
//using Krosoft.Extensions.Data.EntityFramework.Repositories;
//using Krosoft.Extensions.Data.EntityFramework.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace Krosoft.Extensions.Data.EntityFramework.Scopes;

//internal class ReadDbContextScope<T, TTenantId> : IReadDbContextScope where T : KrosoftContext
//{
//    private readonly IServiceScope _serviceScope;
//    protected readonly T DbContext;

//    public ReadDbContextScope(IServiceScope serviceScope,
//                              IDbContextSettings<T> dbContextSettings)
//    {
//        Guard.IsNotNull(nameof(serviceScope), serviceScope);

//        _serviceScope = serviceScope;

//        var krosoftContext = GetContext(serviceScope, dbContextSettings);
//        DbContext = krosoftContext;
//    }

//    public void Dispose()
//    {
//        DbContext.Dispose();
//        _serviceScope.Dispose();
//    }

//    public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

//    public IReadRepository<TEntity> GetReadRepository<TEntity>()
//        where TEntity : class
//        => new ReadRepository<TEntity>(DbContext);

//    private static T GetContext(IServiceScope serviceScope, IDbContextSettings<T>? dbContextSettings)
//    {
//        if (dbContextSettings is IAuditableDbContextSettings<T> auditableDbContextSettings)
//        {
//            var auditableDbContextProvider = new AuditableDbContextProvider(auditableDbContextSettings.Now,
//                                                                            auditableDbContextSettings.UtilisateurId);

//            var krosoftContext = (T?)Activator.CreateInstance(typeof(T),
//                                                              serviceScope.ServiceProvider.GetRequiredService<DbContextOptions>(),
//                                                              auditableDbContextProvider);

//            if (krosoftContext == null)
//            {
//                throw new KrosoftTechnicalException($"Impossible d'instancer le dbcontext de type {typeof(T).Name} avec un settings de type {typeof(IAuditableDbContextSettings<T>).Name}");
//            }

//            return krosoftContext;
//        }

//        if (dbContextSettings is ITenantDbContextSettings<T, TTenantId> tenantDbContextSettings)
//        {
//            var tenantDbContextProvider = new TenantDbContextProvider<TTenantId>(tenantDbContextSettings.TenantId);

//            var krosoftContext = (T?)Activator.CreateInstance(typeof(T),
//                                                              serviceScope.ServiceProvider.GetRequiredService<DbContextOptions>(),
//                                                              tenantDbContextProvider);

//            if (krosoftContext == null)
//            {
//                throw new KrosoftTechnicalException($"Impossible d'instancer le dbcontext de type {typeof(T).Name} avec un settings de type {typeof(ITenantDbContextSettings<T, TTenantId>).Name}");
//            }

//            return krosoftContext;
//        }

//        if (dbContextSettings is ITenantAuditableDbContextSettings<T, TTenantId> tenantAuditableDbContextSettings)
//        {
//            var tenantDbContextProvider = new TenantDbContextProvider<TTenantId>(tenantAuditableDbContextSettings.TenantId);

//            var auditableDbContextProvider = new AuditableDbContextProvider(tenantAuditableDbContextSettings.Now,
//                                                                            tenantAuditableDbContextSettings.UserId);

//            var krosoftContext = (T?)Activator.CreateInstance(typeof(T),
//                                                              serviceScope.ServiceProvider.GetRequiredService<DbContextOptions>(),
//                                                              tenantDbContextProvider,
//                                                              auditableDbContextProvider);

//            if (krosoftContext == null)
//            {
//                throw new KrosoftTechnicalException($"Impossible d'instancer le dbcontext de type {typeof(T).Name} avec un settings de type {typeof(ITenantAuditableDbContextSettings<T, TTenantId>).Name}");
//            }

//            return krosoftContext;
//        }

//        if (dbContextSettings != null)
//        {
//            var krosoftContext = (T?)Activator.CreateInstance(typeof(T),
//                                                              serviceScope.ServiceProvider.GetRequiredService<DbContextOptions>()
//                                                             );

//            if (krosoftContext == null)
//            {
//                throw new KrosoftTechnicalException($"Impossible d'instancer le dbcontext de type {typeof(T).Name} avec un settings de type {typeof(IDbContextSettings<T>).Name}");
//            }

//            return krosoftContext;
//        }

//        throw new KrosoftTechnicalException($"Impossible d'instancer le dbcontext de type {typeof(T).Name}");
//    }
//}







using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Data.EntityFramework.Repositories;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Scopes;

/// <summary>
/// Version simplifiée utilisant l'injection de dépendances
/// </summary>
internal class ReadDbContextScope<T> : IReadDbContextScope where T : KrosoftContext
{
    private readonly IServiceScope _serviceScope;
    protected readonly T DbContext;

    public ReadDbContextScope(IServiceScope serviceScope,
                              IDbContextSettings<T> dbContextSettings)
    {
        Guard.IsNotNull(nameof(serviceScope), serviceScope);

        _serviceScope = serviceScope;
        DbContext = CreateContextWithSettings(serviceScope, dbContextSettings);
    }

    public void Dispose()
    {
        DbContext.Dispose();
        _serviceScope.Dispose();
    }

    public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

    private static T CreateContextWithSettings(IServiceScope serviceScope, IDbContextSettings<T> dbContextSettings)
    {
        // Configure les providers selon les settings avant la création du contexte
        ConfigureProvidersFromSettings(serviceScope.ServiceProvider, dbContextSettings);

        // Le contexte peut maintenant être créé via l'injection de dépendances
        return serviceScope.ServiceProvider.GetRequiredService<T>();
    }

    private static void ConfigureProvidersFromSettings(IServiceProvider serviceProvider, IDbContextSettings<T> dbContextSettings)
    {
        //switch (dbContextSettings)
        //{
        //    //case ITenantAuditableDbContextSettings<T> tenantAuditableSettings:
        //    //    ConfigureTenantProvider(serviceProvider, tenantAuditableSettings.TenantId);
        //    //    ConfigureAuditableProvider(serviceProvider, tenantAuditableSettings.Now, tenantAuditableSettings.UserId);
        //    //    break;

        //    //case IAuditableDbContextSettings<T> auditableSettings:
        //    //    ConfigureAuditableProvider(serviceProvider, auditableSettings.Now, auditableSettings.UtilisateurId);
        //    //    break;

        //    //case ITenantDbContextSettings<T> tenantSettings:
        //    //    ConfigureTenantProvider(serviceProvider, tenantSettings.TenantId);
        //    //    break;
        //}
    }

    //private static void ConfigureTenantProvider(IServiceProvider serviceProvider, object? tenantId)
    //{
    //    // Récupère ou crée un provider de tenant scopé
    //    var tenantProvider = serviceProvider.GetService<ITenantDbContextProvider<object>>() 
    //        ?? new TenantDbContextProvider(tenantId);
            
    //    // Si c'est un provider scopé, met à jour la valeur
    //    if (tenantProvider is ScopedTenantProvider scopedProvider)
    //    {
    //        scopedProvider.SetTenantId(tenantId);
    //    }
    //}

    //private static void ConfigureAuditableProvider(IServiceProvider serviceProvider, DateTimeOffset now, string? userId)
    //{
    //    // Récupère ou crée un provider d'audit scopé
    //    var auditProvider = serviceProvider.GetService<IAuditableDbContextProvider>() 
    //        ?? new ScopedAuditableProvider(now, userId);
            
    //    // Si c'est un provider scopé, met à jour les valeurs
    //    if (auditProvider is ScopedAuditableProvider scopedProvider)
    //    {
    //        scopedProvider.SetAuditInfo(now, userId);
    //    }
    //}

    public IReadRepository<TEntity> GetReadRepository<TEntity>()
        where TEntity : class
        => new ReadRepository<TEntity>(DbContext);
}
 

 
 

/// <summary>
/// Version encore plus simple si vous acceptez de passer par des services résolvables
/// </summary>
internal class SimpleReadDbContextScope<T> : IReadDbContextScope where T : KrosoftContext
{
    private readonly IServiceScope _serviceScope;
    protected readonly T DbContext;

    public SimpleReadDbContextScope(IServiceScope serviceScope)
    {
        Guard.IsNotNull(nameof(serviceScope), serviceScope);
        _serviceScope = serviceScope;
        DbContext = serviceScope.ServiceProvider.GetRequiredService<T>();
    }

    public void Dispose()
    {
        DbContext.Dispose();
        _serviceScope.Dispose();
    }

    public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

    public IReadRepository<TEntity> GetReadRepository<TEntity>()
        where TEntity : class
        => new ReadRepository<TEntity>(DbContext);
}