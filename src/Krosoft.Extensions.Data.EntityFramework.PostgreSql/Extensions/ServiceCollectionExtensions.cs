using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Krosoft.Extensions.Data.EntityFramework.PostgreSql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextPostgreSql<TDbContext>(this IServiceCollection services,
                                                                        IConfiguration configuration,
                                                                        string dbContextName,
                                                                        Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null) where TDbContext : DbContext

    {
        Guard.IsNotNull(nameof(dbContextName), dbContextName);

        var connectionString = configuration.GetConnectionString(dbContextName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new KrosoftTechnicalException($"La ConnectionString basé sur '{dbContextName}' n'est pas définie.");
        }

        services.AddDbContextPostgreSql<TDbContext>(connectionString, npgsqlOptionsAction);

        return services;
    }

    public static IServiceCollection AddDbContextPostgreSql<TDbContext>(this IServiceCollection services,
                                                                        string connectionString,
                                                                        Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null) where TDbContext : DbContext

    {
        Guard.IsNotNull(nameof(connectionString), connectionString);

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddScoped<DbContext, TDbContext>();
        services.AddDbContext<TDbContext>(options =>
                                              options.UseLoggerFactory(LoggerFactoryHelper.MyLoggerFactory)
                                                     .UseNpgsql(connectionString, npgsqlOptionsAction)
                                                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return services;
    }

    public static IServiceCollection AddDbContextPostgreSql<TDbContext>(this IServiceCollection services,
                                                                        IConfiguration configuration,
                                                                        Action<NpgsqlDbContextOptionsBuilder>? npgsqlOptionsAction = null) where TDbContext : DbContext

    {
        var dbContextName = typeof(TDbContext).Name;
        var connectionString = configuration.GetConnectionString(dbContextName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new KrosoftTechnicalException($"La ConnectionString basé sur '{dbContextName}' n'est pas définie.");
        }

        services.AddDbContextPostgreSql<TDbContext>(connectionString, npgsqlOptionsAction);

        return services;
    }
}
