using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Reflection;

namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

/// <summary>
/// Interface pour les configurateurs d'entités
/// </summary>
public interface IEntityConfigurator
{
    bool CanConfigure(Type entityType);
    void Configure(Type entityType, ModelBuilder modelBuilder, object context);
    Type GetInterfaceType();
}

/// <summary>
/// Configurateur pour les entités auditables
/// </summary>
public class AuditableConfigurator : IEntityConfigurator
{
    private readonly IAuditableDbContextProvider _auditableDbContextProvider;
    private static readonly ConcurrentDictionary<Type, MethodInfo> MethodCache = new();

    public AuditableConfigurator(IAuditableDbContextProvider auditableDbContextProvider)
    {
        _auditableDbContextProvider = auditableDbContextProvider;
    }

    public bool CanConfigure(Type entityType) => 
        entityType.GetInterfaces().Contains(typeof(IAuditable));

    public Type GetInterfaceType() => typeof(IAuditable);

    public void Configure(Type entityType, ModelBuilder modelBuilder, object context)
    {
        var method = GetConfigureMethod(context.GetType());
        method.MakeGenericMethod(entityType).Invoke(context, new object[] { modelBuilder });
    }

    private MethodInfo GetConfigureMethod(Type contextType)
    {
        return MethodCache.GetOrAdd(contextType, type =>
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(t => t.IsGenericMethod && t.Name == "ConfigureAuditable"));
    }

    public void ConfigureAuditable<T>(ModelBuilder builder, KrosoftContext context) where T : class, IAuditable
    {
        builder.Entity<T>()
               .Property(t => t.UpdatedBy)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.UpdatedAt)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.CreatedBy)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.CreatedAt)
               .IsRequired();
    }
}

/// <summary>
/// Configurateur pour les entités tenant
/// </summary>
public class TenantConfigurator<TTenantId> : IEntityConfigurator
{
    private readonly ITenantDbContextProvider<TTenantId> _tenantDbContextProvider;
    private static readonly ConcurrentDictionary<Type, MethodInfo> MethodCache = new();

    public TenantConfigurator(ITenantDbContextProvider<TTenantId> tenantDbContextProvider)
    {
        _tenantDbContextProvider = tenantDbContextProvider;
    }

    public bool CanConfigure(Type entityType) => 
        entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITenant<>));

    public Type GetInterfaceType() => typeof(ITenant<TTenantId>);

    public void Configure(Type entityType, ModelBuilder modelBuilder, object context)
    {
        var method = GetConfigureMethod(context.GetType());
        method.MakeGenericMethod(entityType).Invoke(context, new object[] { modelBuilder });
    }

    private MethodInfo GetConfigureMethod(Type contextType)
    {
        return MethodCache.GetOrAdd(contextType, type =>
            type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Single(t => t.IsGenericMethod && t.Name == "ConfigureTenant"));
    }

    public void ConfigureTenant<T>(ModelBuilder builder, KrosoftContext context) where T : class, ITenant<TTenantId>
    {
        builder.Entity<T>()
               .HasIndex(p => p.TenantId);

        builder.Entity<T>()
               .Property(t => t.TenantId)
               .IsRequired();

        builder.Entity<T>()
               .HasQueryFilter(e => e.TenantId != null && e.TenantId.Equals(_tenantDbContextProvider.GetTenantId()));
    }
}

/// <summary>
/// Gestionnaire pour les traitements d'entités lors de la sauvegarde
/// </summary>
public interface IEntityProcessor
{
    bool CanProcess();
    void Process(ChangeTracker changeTracker);
    Type GetInterfaceType();
}

/// <summary>
/// Processeur pour les entités auditables
/// </summary>
public class AuditableProcessor : IEntityProcessor
{
    private readonly IAuditableDbContextProvider _auditableDbContextProvider;

    public AuditableProcessor(IAuditableDbContextProvider auditableDbContextProvider)
    {
        _auditableDbContextProvider = auditableDbContextProvider;
    }

    public Type GetInterfaceType() => typeof(IAuditable);

    public bool CanProcess() => true; // Toujours disponible si injecté

    public void Process(ChangeTracker changeTracker)
    {
        var useAudit = changeTracker.Entries<IAuditable>().Any();
        if (useAudit)
        {
            changeTracker.DetectChanges();

            var now = _auditableDbContextProvider.GetNow();
            var userId = _auditableDbContextProvider.GetUserId();

            changeTracker.ProcessModificationAuditable(now, userId);
            changeTracker.ProcessCreationAuditable(now, userId);
        }
    }
}

/// <summary>
/// Processeur pour les entités tenant
/// </summary>
public class TenantProcessor<TTenantId> : IEntityProcessor
{
    private readonly ITenantDbContextProvider<TTenantId> _tenantDbContextProvider;

    public TenantProcessor(ITenantDbContextProvider<TTenantId> tenantDbContextProvider)
    {
        _tenantDbContextProvider = tenantDbContextProvider;
    }

    public Type GetInterfaceType() => typeof(ITenant<TTenantId>);

    public bool CanProcess() => true; // Toujours disponible si injecté

    public void Process(ChangeTracker changeTracker)
    {
        var useTenant = changeTracker.Entries<ITenant<TTenantId>>().Any();
        if (useTenant)
        {
            changeTracker.DetectChanges();

            var tenantId = _tenantDbContextProvider.GetTenantId();
            changeTracker.ProcessCreationTenant(tenantId);
        }
    }
}

/// <summary>
/// Contexte de base utilisant les stratégies
/// </summary>
public abstract class KrosoftStrategyContext : KrosoftContext
{
    private readonly IList<IEntityConfigurator> _configurators;
    private readonly IList<IEntityProcessor> _processors;

    protected KrosoftStrategyContext(DbContextOptions options,
                                     IEnumerable<IEntityConfigurator> configurators,
                                     IEnumerable<IEntityProcessor> processors) : base(options)
    {
        _configurators = configurators.ToList();
        _processors = processors.ToList();
    }

    protected override IEnumerable<Type> GetTypes() => 
        _configurators.Select(c => c.GetInterfaceType()).Distinct();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var type in GetEntityTypes())
        {
            foreach (var configurator in _configurators.Where(c => c.CanConfigure(type)))
            {
                configurator.Configure(type, modelBuilder, this);
            }
        }
    }

    protected override void OverrideEntities()
    {
        foreach (var processor in _processors.Where(p => p.CanProcess()))
        {
            processor.Process(ChangeTracker);
        }
    }

    // Méthodes de configuration - appelées par réflexion
    public void ConfigureAuditable<T>(ModelBuilder builder) where T : class, IAuditable
    {
        var configurator = _configurators.OfType<AuditableConfigurator>().FirstOrDefault();
        configurator?.ConfigureAuditable<T>(builder, this);
    }

    public void ConfigureTenant<T>(ModelBuilder builder) where T : class, ITenant<object>
    {
        // Cette méthode sera surchargée dans les classes dérivées avec le bon type
    }
}
 

/// <summary>
/// Classe de base pour les contextes supportant la configuration automatique d'entités
/// basée sur les interfaces IAuditable et ITenant
/// </summary>
public abstract class KrosoftConfigurableContext<TTenantId> : KrosoftContext
{
    // Cache des méthodes de configuration par type de contexte
    private static readonly ConcurrentDictionary<Type, ConfigurationMethods> MethodCache 
        = new ConcurrentDictionary<Type, ConfigurationMethods>();

    private readonly IAuditableDbContextProvider? _auditableDbContextProvider;
    private readonly ITenantDbContextProvider<TTenantId>? _tenantDbContextProvider;

    protected KrosoftConfigurableContext(DbContextOptions options,
                                        ITenantDbContextProvider<TTenantId>? tenantDbContextProvider = null,
                                        IAuditableDbContextProvider? auditableDbContextProvider = null) : base(options)
    {
        _auditableDbContextProvider = auditableDbContextProvider;
        _tenantDbContextProvider = tenantDbContextProvider;
    }

    /// <summary>
    /// Indique si ce contexte supporte la fonctionnalité d'audit
    /// </summary>
    protected virtual bool SupportsAuditing => _auditableDbContextProvider != null;

    /// <summary>
    /// Indique si ce contexte supporte la fonctionnalité multi-tenant
    /// </summary>
    protected virtual bool SupportsTenancy => _tenantDbContextProvider != null;

    /// <summary>
    /// Configure les propriétés d'audit pour une entité
    /// </summary>
    public void ConfigureAuditable<T>(ModelBuilder builder) where T : class, IAuditable
    {
        if (!SupportsAuditing)
            return;

        builder.Entity<T>()
               .Property(t => t.UpdatedBy)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.UpdatedAt)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.CreatedBy)
               .IsRequired();
        builder.Entity<T>()
               .Property(t => t.CreatedAt)
               .IsRequired();
    }

    /// <summary>
    /// Configure les propriétés de tenant pour une entité
    /// </summary>
    public void ConfigureTenant<T>(ModelBuilder builder) where T : class, ITenant<TTenantId>
    {
        if (!SupportsTenancy || _tenantDbContextProvider == null)
            return;

        builder.Entity<T>()
               .HasIndex(p => p.TenantId);

        builder.Entity<T>()
               .Property(t => t.TenantId)
               .IsRequired();

        builder.Entity<T>()
               .HasQueryFilter(e => e.TenantId != null && e.TenantId.Equals(_tenantDbContextProvider.GetTenantId()));
    }

    protected override IEnumerable<Type> GetTypes()
    {
        var types = new List<Type>();
        
        if (SupportsTenancy)
            types.Add(typeof(ITenant<TTenantId>));
        
        if (SupportsAuditing)
            types.Add(typeof(IAuditable));
            
        return types;
    }

    private ConfigurationMethods GetConfigurationMethods()
    {
        return MethodCache.GetOrAdd(GetType(), contextType =>
        {
            MethodInfo? configureAuditableMethod = null;
            MethodInfo? configureTenantMethod = null;

            if (SupportsAuditing)
            {
                configureAuditableMethod = contextType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .SingleOrDefault(t => t.IsGenericMethod && t.Name == nameof(ConfigureAuditable));
            }

            if (SupportsTenancy)
            {
                configureTenantMethod = contextType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .SingleOrDefault(t => t.IsGenericMethod && t.Name == nameof(ConfigureTenant));
            }

            return new ConfigurationMethods(configureAuditableMethod, configureTenantMethod);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var configMethods = GetConfigurationMethods();

        foreach (var type in GetEntityTypes())
        {
            // Configuration audit
            if (SupportsAuditing && 
                configMethods.ConfigureAuditable != null &&
                type.GetInterfaces().Contains(typeof(IAuditable)))
            {
                var method = configMethods.ConfigureAuditable.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }

            // Configuration tenant
            if (SupportsTenancy && 
                configMethods.ConfigureTenant != null &&
                type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITenant<>)))
            {
                var method = configMethods.ConfigureTenant.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    protected override void OverrideEntities()
    {
        // Gestion de l'audit
        if (SupportsAuditing && _auditableDbContextProvider != null)
        {
            var useAudit = ChangeTracker.Entries<IAuditable>().Any();
            if (useAudit)
            {
                ChangeTracker.DetectChanges();

                var now = _auditableDbContextProvider.GetNow();
                var userId = _auditableDbContextProvider.GetUserId();

                ChangeTracker.ProcessModificationAuditable(now, userId);
                ChangeTracker.ProcessCreationAuditable(now, userId);
            }
        }

        // Gestion du tenant
        if (SupportsTenancy && _tenantDbContextProvider != null)
        {
            var useTenant = ChangeTracker.Entries<ITenant<TTenantId>>().Any();
            if (useTenant)
            {
                ChangeTracker.DetectChanges();

                var tenantId = _tenantDbContextProvider.GetTenantId();
                ChangeTracker.ProcessCreationTenant(tenantId);
            }
        }
    }

    /// <summary>
    /// Structure pour stocker les méthodes de configuration en cache
    /// </summary>
    private record ConfigurationMethods(MethodInfo? ConfigureAuditable, MethodInfo? ConfigureTenant);
}

/// <summary>
/// Contexte supportant uniquement la fonctionnalité tenant
/// </summary>
public abstract class KrosoftTenantContext<TTenantId> : KrosoftConfigurableContext<TTenantId>
{
    protected KrosoftTenantContext(DbContextOptions options,
                                   ITenantDbContextProvider<TTenantId> tenantDbContextProvider) 
        : base(options, tenantDbContextProvider)
    {
    }
}

/// <summary>
/// Contexte supportant les fonctionnalités tenant et audit
/// </summary>
public abstract class KrosoftTenantAuditableContext<TTenantId> : KrosoftConfigurableContext<TTenantId>
{
    protected KrosoftTenantAuditableContext(DbContextOptions options,
                                            ITenantDbContextProvider<TTenantId> tenantDbContextProvider,
                                            IAuditableDbContextProvider auditableDbContextProvider) 
        : base(options, tenantDbContextProvider, auditableDbContextProvider)
    {
    }
}

/// <summary>
/// Contexte supportant uniquement la fonctionnalité audit
/// </summary>
public abstract class KrosoftAuditableContext : KrosoftConfigurableContext<object>
{
    protected KrosoftAuditableContext(DbContextOptions options,
                                      IAuditableDbContextProvider auditableDbContextProvider) 
        : base(options, null, auditableDbContextProvider)
    {
    }
}