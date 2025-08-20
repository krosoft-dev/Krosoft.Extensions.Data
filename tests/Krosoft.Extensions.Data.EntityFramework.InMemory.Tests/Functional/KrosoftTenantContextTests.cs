using JetBrains.Annotations;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.InMemory.Tests.Functional;

[TestClass]
[TestSubject(typeof(KrosoftTenantContext))]
public class KrosoftTenantContextTests : BaseTest
{
    private IReadRepository<Logiciel> _repository = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingExt();
        services.AddRepositories();
        services.AddScoped<ITenantDbContextProvider, FakeTenantDbContextProvider>();
        services.AddDbContextInMemory<SampleKrosoftTenantContext>(true);
        services.AddSeedService<SampleKrosoftTenantContext, SampleSeedService<SampleKrosoftTenantContext>>();
    }

    [TestCleanup]
    public void Cleanup() => _repository.Dispose();

    [TestMethod]
    public async Task Query_Ok()
    {
        var logiciels = await _repository.Query()
                                         .ToListAsync(CancellationToken.None);

        Check.That(logiciels).IsNotNull();
        Check.That(logiciels).HasSize(5);
        Check.That(logiciels.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2", "Logiciel3", "Logiciel4", "Logiciel5");
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _repository = serviceProvider.GetRequiredService<IReadRepository<Logiciel>>();
    }
}