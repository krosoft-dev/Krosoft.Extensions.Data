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
[TestSubject(typeof(KrosoftAuditableContext))]
public class KrosoftAuditableContextTests : BaseTest
{
    private IReadRepository<Pays>? _repository;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingExt();
        services.AddRepositories();
        services.AddScoped<IAuditableDbContextProvider, FakeAuditableDbContextProvider>();
        services.AddDbContextInMemory<SampleKrosoftAuditableContext>(true);
        services.AddSeedService<SampleKrosoftAuditableContext, SampleSeedService<SampleKrosoftAuditableContext>>();
    }

    [TestCleanup]
    public void Cleanup() => _repository?.Dispose();

    [TestMethod]
    public async Task Query_Ok()
    {
        var pays = await _repository!.Query()
                                     .ToListAsync(CancellationToken.None);

        Check.That(pays).IsNotNull();
        Check.That(pays).HasSize(5);
        Check.That(pays.Select(x => x.Code)).ContainsExactly("fr", "de", "it", "es", "gb");
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _repository = serviceProvider.GetRequiredService<IReadRepository<Pays>>();
    }
}