using JetBrains.Annotations;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.InMemory.Tests.Functional;

[TestClass]
[TestSubject(typeof(KrosoftContext))]
public class KrosoftContextTests : BaseTest
{
    private IReadRepository<Langue> _repository = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingExt();
        services.AddRepositories();
        services.AddDbContextInMemory<SampleKrosoftContext>(true);
        services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
    }

    [TestCleanup]
    public void Cleanup() => _repository.Dispose();

    [TestMethod]
    public async Task Query_Ok()
    {
        var langues = await _repository.Query()
                                       .ToListAsync(CancellationToken.None);

        Check.That(langues).IsNotNull();
        Check.That(langues).HasSize(2);
        Check.That(langues.Select(x => x.Code)).ContainsExactly("fr", "en");
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _repository = serviceProvider.GetRequiredService<IReadRepository<Langue>>();
    }
}