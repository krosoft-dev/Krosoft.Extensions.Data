using System.Reflection;
using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Contexts;

public abstract class KrosoftAuditableContext : KrosoftContext
{
    private static readonly MethodInfo ConfigureAuditableMethod = typeof(KrosoftAuditableContext)
                                                                  .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                                  .Single(t => t.IsGenericMethod && t.Name == nameof(ConfigureAuditable));

    private readonly IAuditableDbContextProvider _auditableDbContextProvider;

    protected KrosoftAuditableContext(DbContextOptions options,
                                      IAuditableDbContextProvider auditableDbContextProvider) : base(options)
    {
        _auditableDbContextProvider = auditableDbContextProvider;
    }

    public void ConfigureAuditable<T>(ModelBuilder builder) where T : class, IAuditable
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

    protected override IEnumerable<Type> GetTypes() => [typeof(IAuditable)];

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set BaseEntity rules to all loaded entity types
        foreach (var type in GetEntityTypes())
        {
            //Console.WriteLine(type.FullName); //Debug.

            if (type.GetInterfaces().Contains(typeof(IAuditable)))
            {
                var method = ConfigureAuditableMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    protected override void OverrideEntities()
    {
        var useAudit = ChangeTracker.Entries<IAuditable>().Any();
        if (useAudit)
        {
            ChangeTracker.DetectChanges();

            var now = _auditableDbContextProvider.GetNow();
            var userId = _auditableDbContextProvider.GetUserId();

            ChangeTracker.ProcessAuditableOnAdded(now, userId);
            ChangeTracker.ProcessAuditableOnModified(now, userId);
        }
    }
}