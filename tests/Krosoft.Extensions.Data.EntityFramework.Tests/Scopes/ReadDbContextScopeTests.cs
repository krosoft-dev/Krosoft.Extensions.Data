using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Data.EntityFramework.Scopes;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Scopes;

[TestClass]
public class ReadDbContextScopeTests : BaseTest
{
    private static async Task CheckResults<T>(ReadDbContextScope<T> contextScope) where T : KrosoftContext
    {
        var repository = contextScope.GetReadRepository<Logiciel>();

        var logiciels = await repository.Query()
                                        .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(5);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestMethod]
    public async Task ReadDbContextScope_NoTenantAudit_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftAuditableContext>(true);
            services.AddSeedService<SampleKrosoftAuditableContext, SampleSeedService<SampleKrosoftAuditableContext>>();
        }

        using (var scope = CreateServiceCollection(GetServices))
        {
            var dbContextSettings = new AuditableDbContextSettings<SampleKrosoftAuditableContext>(DateTime.Now,
                                                                                                  "UtilisateurId");
            using (var contextScope = new ReadDbContextScope<SampleKrosoftAuditableContext>(scope.CreateScope(), dbContextSettings))
            {
                await CheckResults(contextScope);
            }
        }
    }

    [TestMethod]
    public async Task ReadDbContextScope_NoTenantNoAudit_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddDbContextInMemory<SampleKrosoftContext>(true);
            services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
        }

        using (var scope = CreateServiceCollection(GetServices))
        {
            using (var contextScope = new ReadDbContextScope<SampleKrosoftContext>(scope.CreateScope(), new DbContextSettings<SampleKrosoftContext>()))
            {
                await CheckResults(contextScope);
            }
        }
    }

    [TestMethod]
    public async Task ReadDbContextScope_TenantAudit_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
            services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftTenantAuditableContext>(true);
            services.AddSeedService<SampleKrosoftTenantAuditableContext, SampleSeedService<SampleKrosoftTenantAuditableContext>>();
        }

        using (var scope = CreateServiceCollection(GetServices))
        {
            var tenantId = new FakeTenantDbContextProvider().GetTenantId();
            var dbContextSettings = new TenantAuditableDbContextSettings<SampleKrosoftTenantAuditableContext>(tenantId,
                                                                                                              DateTime.Now,
                                                                                                              "UtilisateurId");
            using (var contextScope = new ReadDbContextScope<SampleKrosoftTenantAuditableContext>(scope.CreateScope(), dbContextSettings))
            {
                await CheckResults(contextScope);
            }
        }
    }

    [TestMethod]
    public async Task ReadDbContextScope_TenantNoAudit_Ok()
    {
        void GetServices(IServiceCollection services)
        {
            services.AddRepositories();
            services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
            services.AddDbContextInMemory<SampleKrosoftTenantContext>(true);
            services.AddSeedService<SampleKrosoftTenantContext, SampleSeedService<SampleKrosoftTenantContext>>();
        }

        using (var scope = CreateServiceCollection(GetServices))
        {
            var tenantId = new FakeTenantDbContextProvider().GetTenantId();
            var dbContextSettings = new TenantDbContextSettings<SampleKrosoftTenantContext>(tenantId);
            using (var contextScope = new ReadDbContextScope<SampleKrosoftTenantContext>(scope.CreateScope(), dbContextSettings))
            {
                await CheckResults(contextScope);
            }
        }
    }
}