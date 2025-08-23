using Krosoft.Extensions.Core.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Data.EntityFramework.Services;

public abstract class DesignTimeDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
{
    public TDbContext CreateDbContext(string[] args)
    {
        var dbContextName = typeof(TDbContext).Name;
        Console.WriteLine($"DbContextName :  {dbContextName}");

        var configurationBuilder = new ConfigurationBuilder()
                                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                                   .AddJsonFile("appsettings.json", false, true);

        if (args.Length >= 1)
        {
            var environment = args[0];
            configurationBuilder.AddJsonFile($"appsettings.{environment}.json", true);
            Console.WriteLine($"Environment :  {environment}");
        }

        var configuration = configurationBuilder.Build();
        var connectionString = configuration.GetConnectionString(dbContextName);
        Console.WriteLine($"ConnectionString :  {connectionString}");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new KrosoftTechnicalException($"Impossible de définir la connectionString à partir du nom du DbContext '{dbContextName}'.");
        }

        var services = new ServiceCollection();
        ConfigureServices(services);
        services.AddDbContext<TDbContext>(options => UseProvider(options, connectionString));

        var context = services.BuildServiceProvider()
                              .GetRequiredService<TDbContext>();
        return context;
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ITenantDbContextProvider<string>, FakeTenantDbContextProvider>();
        services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
    }

    protected abstract void UseProvider(DbContextOptionsBuilder options, string connectionString);
}