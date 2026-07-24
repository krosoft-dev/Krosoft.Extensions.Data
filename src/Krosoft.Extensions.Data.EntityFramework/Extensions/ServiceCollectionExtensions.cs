using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Repositories;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Krosoft.Extensions.Data.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly object DbLock = new();

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGenericWriteRepository, GenericWriteRepository>();
        services.AddScoped<IGenericReadRepository, GenericReadRepository>();
        services.AddScoped<IBatchDbService, BatchDbService>();

        return services;
    }

    public static IServiceCollection AddSeedService<TDbContext, TSeedService>(this IServiceCollection services) where TDbContext : DbContext where TSeedService : class, ISeedService<TDbContext>
    {
        services.AddScoped<ISeedService<TDbContext>, TSeedService>();

        services.AddLogging();
        // Build the service provider.
        var sp = services.BuildServiceProvider();

        // Create a scope to obtain a reference to the database context (ApplicationDbContext).
        using (var scope = sp.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<TDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<TDbContext>>();
            var seedService = scopedServices.GetRequiredService<ISeedService<TDbContext>>();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            try
            {
                lock (DbLock)
                {
                    if (!seedService.Initialized)
                    {
                        logger.LogInformation("Seed de la DB InMemory");

                        seedService.InitializeDbForTests(db);
                    }
                    else
                    {
                        logger.LogWarning("Seed déjà OK.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error seed :{ex.Message}");

                throw;
            }
        }

        return services;
    }
}