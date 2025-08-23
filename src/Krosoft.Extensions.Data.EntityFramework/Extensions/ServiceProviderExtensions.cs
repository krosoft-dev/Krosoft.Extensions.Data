using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Data.EntityFramework.Scopes;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class ServiceProviderExtensions
{
    public static IDbContextScope CreateDbContextScope<T, TTenantId>(this IServiceProvider provider,
                                                                     TTenantId tenantId,
                                                                     DateTime now,
                                                                     string userId)
        where T : KrosoftTenantAuditableContext<TTenantId>
        => CreateDbContextScope(provider,
                                new TenantAuditableDbContextSettings<T, TTenantId>(tenantId,
                                                                                   now,
                                                                                   userId));

    public static IDbContextScope CreateDbContextScope<T>(this IServiceProvider provider,
                                                          IDbContextSettings<T> dbContextSettings)
        where T : KrosoftContext
        => new DbContextScope<T>(provider.CreateScope(),
                                 dbContextSettings);

    public static IReadDbContextScope CreateReadDbContextScope<T>(this IServiceProvider provider,
                                                                  IDbContextSettings<T> dbContextSettings)
        where T : KrosoftContext
        => new ReadDbContextScope<T>(provider.CreateScope(),
                                     dbContextSettings);
}