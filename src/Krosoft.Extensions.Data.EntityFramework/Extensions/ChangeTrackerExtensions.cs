using Krosoft.Extensions.Data.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class ChangeTrackerExtensions
{
    public static void ProcessCreationTenant<T>(this ChangeTracker changeTracker,
                                                T tenantId)  
    {
        foreach (var item in changeTracker.Entries<ITenant<T>>().Where(e => e.State == EntityState.Added))
        {
            item.Entity.TenantId = tenantId;
        }
    }

    public static void ProcessCreationAuditable(this ChangeTracker changeTracker,
                                                DateTimeOffset now,
                                                string userId)
    {
        foreach (var item in changeTracker.Entries<IAuditable>()
                                          .Where(e => e.State == EntityState.Added))
        {
            item.Entity.CreatedBy = userId;
            item.Entity.CreatedAt = now;
            item.Entity.UpdatedBy = userId;
            item.Entity.UpdatedAt = now;
        }
    }

    public static void ProcessModificationAuditable(this ChangeTracker changeTracker,
                                                    DateTimeOffset now,
                                                    string userId)
    {
        foreach (var item in changeTracker.Entries<IAuditable>()
                                          .Where(e => e.State == EntityState.Modified))
        {
            item.Entity.UpdatedBy = userId;
            item.Entity.UpdatedAt = now;
        }
    }
}