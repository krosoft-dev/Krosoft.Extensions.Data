using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Repositories;

using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Repositories;

[TestClass]
[TestSubject(typeof(GenericReadRepository))]
public class GenericReadRepositoryTests : BaseTest
{
    //TestInitialize
    private IGenericReadRepository _repository = null!;

    protected override void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddDbContextInMemory<SampleKrosoftContext>(true);
        services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
    }

    [TestMethod]
    public async Task Query_Ok()
    {
        var langues = await _repository.Query<Langue>()
                                       .ToListAsync(CancellationToken.None);

        Check.That(langues).IsNotNull();
        Check.That(langues).HasSize(2);
        Check.That(langues.Select(x => x.Code)).ContainsExactly("fr", "en");
    }

    [TestInitialize]
    public void SetUp()
    {
        var serviceProvider = CreateServiceCollection();
        _repository = serviceProvider.GetRequiredService<IGenericReadRepository>();
    }

    [TestMethod]
    public async Task ToListAsync_Ok()
    {
        var query = _repository.Query<Langue>();
        var langues = await _repository.ToListAsync(query, CancellationToken.None)!.ToList();

        Check.That(langues).IsNotNull();
        Check.That(langues).HasSize(2);
        Check.That(langues.Select(x => x.Code)).ContainsExactly("fr", "en");
    }
}