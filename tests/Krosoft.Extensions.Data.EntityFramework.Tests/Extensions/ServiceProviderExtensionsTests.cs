using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Extensions;

[TestClass]
public class ServiceProviderExtensionsTests : BaseTest
{
    [TestMethod]
    public async Task CreateDbContextScope_Auditable_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftAuditableContext>(true);
            services.AddSeedService<SampleKrosoftAuditableContext, SampleSeedService<SampleKrosoftAuditableContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateDbContextScope(new AuditableDbContextSettings<SampleKrosoftAuditableContext>(DateTime.Now, ""));

        var repository = contextScope.GetWriteRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(7);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Adobe Acrobat Reader", "Microsoft Excel", "Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateDbContextScope_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();
            services.AddDbContextInMemory<SampleKrosoftContext>(true);
            services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateDbContextScope(new DbContextSettings<SampleKrosoftContext>());

        var repository = contextScope.GetWriteRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(7);
        Check.That(logiciels.Select(x => x.Nom))
             .ContainsExactly("Adobe Acrobat Reader", "Microsoft Excel", "Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateDbContextScope_TenantAuditable_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();
            services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftTenantAuditableContext>(true);
            services.AddSeedService<SampleKrosoftTenantAuditableContext, SampleSeedService<SampleKrosoftTenantAuditableContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateDbContextScope(new TenantAuditableDbContextSettings<SampleKrosoftTenantAuditableContext>(new FakeTenantDbContextProvider().GetTenantId(), DateTime.Now, ""));

        var repository = contextScope.GetWriteRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(5);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateReadDbContextScope_Auditable_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftAuditableContext>(true);
            services.AddSeedService<SampleKrosoftAuditableContext, SampleSeedService<SampleKrosoftAuditableContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateReadDbContextScope(new AuditableDbContextSettings<SampleKrosoftAuditableContext>(DateTime.Now, ""));

        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(7);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Adobe Acrobat Reader", "Microsoft Excel", "Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateReadDbContextScope_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();
            services.AddDbContextInMemory<SampleKrosoftContext>(true);
            services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateReadDbContextScope(new DbContextSettings<SampleKrosoftContext>());

        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(7);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Adobe Acrobat Reader", "Microsoft Excel", "Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateReadDbContextScope_TenantAuditable_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();
            services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftTenantAuditableContext>(true);
            services.AddSeedService<SampleKrosoftTenantAuditableContext, SampleSeedService<SampleKrosoftTenantAuditableContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateReadDbContextScope(new TenantAuditableDbContextSettings<SampleKrosoftTenantAuditableContext>(new FakeTenantDbContextProvider().GetTenantId(), DateTime.Now, ""));

        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(5);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task CreateReadDbContextScope_Tenant_Empty()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories(); 
            services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftTenantAuditableContext>(true);
            services.AddSeedService<SampleKrosoftTenantAuditableContext, SampleSeedService<SampleKrosoftTenantAuditableContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateReadDbContextScope(new TenantAuditableDbContextSettings<SampleKrosoftTenantAuditableContext>("test", DateTime.Now, ""));

        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsEmpty();
    }

    [TestMethod]
    public async Task CreateReadDbContextScope_Tenant_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddLoggingExt();
            services.AddRepositories();  
            services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftTenantAuditableContext>(true);
            services.AddSeedService<SampleKrosoftTenantAuditableContext, SampleSeedService<SampleKrosoftTenantAuditableContext>>();
        }

        await using var serviceProvider = CreateServiceCollection(GetServices);
        using var contextScope = serviceProvider.CreateReadDbContextScope(new TenantAuditableDbContextSettings<SampleKrosoftTenantAuditableContext>("Microsoft", DateTime.Now, ""));

        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(1);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Microsoft Excel");
    }
}