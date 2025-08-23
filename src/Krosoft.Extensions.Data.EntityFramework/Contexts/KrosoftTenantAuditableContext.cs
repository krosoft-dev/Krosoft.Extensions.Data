//using System.Collections.Concurrent;
//using System.Reflection;
//using Krosoft.Extensions.Data.Abstractions.Models;
//using Krosoft.Extensions.Data.EntityFramework.Extensions;
//using Krosoft.Extensions.Data.EntityFramework.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

//public abstract class KrosoftTenantAuditableContext<TTenantId> : KrosoftContext
//{
//    private static readonly ConcurrentDictionary<Type, (MethodInfo ConfigureAuditable, MethodInfo ConfigureTenant)> MethodCache
//        = new ConcurrentDictionary<Type, (MethodInfo, MethodInfo)>();

//    private readonly IAuditableDbContextProvider _auditableDbContextProvider;
//    private readonly ITenantDbContextProvider<TTenantId> _tenantDbContextProvider;

//    protected KrosoftTenantAuditableContext(DbContextOptions options,
//                                            ITenantDbContextProvider<TTenantId> tenantDbContextProvider,
//                                            IAuditableDbContextProvider auditableDbContextProvider) : base(options)
//    {
//        _auditableDbContextProvider = auditableDbContextProvider;
//        _tenantDbContextProvider = tenantDbContextProvider;
//    }

//    /// <summary>
//    /// This method is called for every loaded entity type in OnModelCreating method.
//    /// Here type is known through generic parameter and we can use EF Core methods.
//    /// </summary>
//    public void ConfigureAuditable<T>(ModelBuilder builder) where T : class, IAuditable
//    {
//        builder.Entity<T>()
//               .Property(t => t.UpdatedBy)
//               .IsRequired();
//        builder.Entity<T>()
//               .Property(t => t.UpdatedAt)
//               .IsRequired();
//        builder.Entity<T>()
//               .Property(t => t.CreatedBy)
//               .IsRequired();
//        builder.Entity<T>()
//               .Property(t => t.CreatedAt)
//               .IsRequired();
//    }

//    /// <summary>
//    /// This method is called for every loaded entity type in OnModelCreating method.
//    /// Here type is known through generic parameter and we can use EF Core methods.
//    /// </summary>
//    public void ConfigureTenant<T>(ModelBuilder builder) where T : class, ITenant<TTenantId>
//    {
//        builder.Entity<T>()
//               .HasIndex(p => p.TenantId);

//        builder.Entity<T>()
//               .Property(t => t.TenantId)
//               .IsRequired();

//        builder.Entity<T>().HasQueryFilter(e => e.TenantId != null && e.TenantId.Equals(_tenantDbContextProvider.GetTenantId()));
//    }

//    protected override IEnumerable<Type> GetTypes() => [typeof(ITenant<TTenantId>), typeof(IAuditable)];

//    private (MethodInfo ConfigureAuditable, MethodInfo ConfigureTenant) GetConfigurationMethods()
//    {
//        return MethodCache.GetOrAdd(GetType(), contextType =>
//        {
//            var configureAuditableMethod = contextType
//                                           .GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                                           .Single(t => t.IsGenericMethod && t.Name == nameof(ConfigureAuditable));

//            var configureTenantMethod = contextType
//                                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                                        .Single(t => t.IsGenericMethod && t.Name == nameof(ConfigureTenant));

//            return (configureAuditableMethod, configureTenantMethod);
//        });
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        var (configureAuditableMethod, configureTenantMethod) = GetConfigurationMethods();

//        // Set BaseEntity rules to all loaded entity types
//        foreach (var type in GetEntityTypes())
//        {
//            //Console.WriteLine(type.FullName); //Debug.

//            if (type.GetInterfaces().Contains(typeof(IAuditable)))
//            {
//                var method = configureAuditableMethod.MakeGenericMethod(type);
//                method.Invoke(this, new object[] { modelBuilder });
//            }

//            if (type.GetInterfaces().Contains(typeof(ITenant<TTenantId>)))
//            {
//                var method = configureTenantMethod.MakeGenericMethod(type);
//                method.Invoke(this, new object[] { modelBuilder });
//            }
//        }
//    }

//    protected override void OverrideEntities()
//    {
//        var useAudit = ChangeTracker.Entries<IAuditable>().Any();
//        if (useAudit)
//        {
//            ChangeTracker.DetectChanges();

//            var now = _auditableDbContextProvider.GetNow();
//            var userId = _auditableDbContextProvider.GetUserId();

//            ChangeTracker.ProcessModificationAuditable(now, userId);
//            ChangeTracker.ProcessCreationAuditable(now, userId);
//        }

//        var useTenant = ChangeTracker.Entries<ITenant<TTenantId>>().Any();
//        if (useTenant)
//        {
//            ChangeTracker.DetectChanges();

//            var tenantId = _tenantDbContextProvider.GetTenantId();
//            ChangeTracker.ProcessCreationTenant(tenantId);
//        }
//    }
//}