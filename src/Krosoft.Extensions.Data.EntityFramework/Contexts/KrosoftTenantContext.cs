//using System.Collections.Concurrent;
//using System.Reflection;
//using Krosoft.Extensions.Data.Abstractions.Models;
//using Krosoft.Extensions.Data.EntityFramework.Extensions;
//using Krosoft.Extensions.Data.EntityFramework.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

//public abstract class KrosoftTenantContext<TTenantId> : KrosoftContext
//{
//    // Cache the methods per concrete type for better performance
//    private static readonly ConcurrentDictionary<Type, MethodInfo> MethodCache
//        = new ConcurrentDictionary<Type, MethodInfo>();

//    private readonly ITenantDbContextProvider<TTenantId> _tenantDbContextProvider;

//    protected KrosoftTenantContext(DbContextOptions options,
//                                   ITenantDbContextProvider<TTenantId> tenantDbContextProvider) : base(options)
//    {
//        _tenantDbContextProvider = tenantDbContextProvider;
//    }

//    /// <summary>
//    /// This method is called for every loaded entity type in OnModelCreating method.
//    /// Here type is known through generic parameter and we can use EF Core methods.
//    /// </summary>
//    public void ConfigureTenant<T>(ModelBuilder builder)
//        where T : class, ITenant<TTenantId>
//    {
//        builder.Entity<T>()
//               .HasIndex(p => p.TenantId);

//        builder.Entity<T>()
//               .Property(t => t.TenantId)
//               .IsRequired();

//        builder.Entity<T>().HasQueryFilter(e => e.TenantId != null && e.TenantId.Equals(_tenantDbContextProvider.GetTenantId()));
//    }

//    protected override IEnumerable<Type> GetTypes() => [typeof(ITenant<TTenantId>)];

//    private MethodInfo GetConfigureTenantMethod()
//    {
//        return MethodCache.GetOrAdd(GetType(), contextType =>
//        {
//            return contextType
//                   .GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                   .Single(t => t.IsGenericMethod && t.Name == nameof(ConfigureTenant));
//        });
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);

//        var configureTenantMethod = GetConfigureTenantMethod();

//        // Set BaseEntity rules to all loaded entity types
//        foreach (var type in GetEntityTypes())
//        {
//            //Console.WriteLine(type.FullName); //Debug.

//            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITenant<>)))
//            {
//                var method = configureTenantMethod.MakeGenericMethod(type);
//                method.Invoke(this, new object[] { modelBuilder });
//            }
//        }
//    }

//    protected override void OverrideEntities()
//    {
//        var useTenant = ChangeTracker.Entries<ITenant<TTenantId>>().Any();
//        if (useTenant)
//        {
//            ChangeTracker.DetectChanges();

//            var tenantId = _tenantDbContextProvider.GetTenantId();
//            ChangeTracker.ProcessCreationTenant(tenantId);
//        }
//    }
//}

using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

public abstract class KrosoftTenantContext<TTenantId> : KrosoftConfigurableContext<TTenantId>
{
    protected KrosoftTenantContext(DbContextOptions options,
                                   ITenantDbContextProvider<TTenantId> tenantDbContextProvider) 
        : base(options, tenantDbContextProvider)
    {
    }
}