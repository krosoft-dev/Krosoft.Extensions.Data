//using JetBrains.Annotations;
//using Krosoft.Extensions.Data.EntityFramework.Repositories;
//using Krosoft.Extensions.Testing;

using JetBrains.Annotations;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Repositories;

using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.Testing.Data.EntityFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Repositories;

[TestClass]
[TestSubject(typeof(GenericWriteRepository))]
public class GenericWriteRepositoryTests : BaseTest
{
    [TestMethod]
    public void Delete_Ok()
    {
        using var serviceProvider = CreateServiceCollection(GetServices);
        var repository = serviceProvider.GetRequiredService<IGenericWriteRepository>();
        using var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        Check.That(this.GetDb<Langue>(serviceProvider).Count()).IsEqualTo(2);

        var toDelete = this.GetDb<Langue>(serviceProvider).First();

        repository.Delete(toDelete);
        var nb = unitOfWork.SaveChanges();

        Check.That(nb).IsEqualTo(1);
        Check.That(this.GetDb<Langue>(serviceProvider).Count()).IsEqualTo(1);
    }

    [TestMethod]
    public void GetAsync_Ok()
    {
        Assert.Inconclusive();
    }

    private void GetServices(IServiceCollection services)
    {
        services.AddRepositories();
        services.AddDbContextInMemory<SampleKrosoftContext>(true);
        services.AddSeedService<SampleKrosoftContext, SampleSeedService<SampleKrosoftContext>>();
    }

    [TestMethod]
    public void Insert_Ok()
    {
        using var serviceProvider = CreateServiceCollection(GetServices);
        var repository = serviceProvider.GetRequiredService<IGenericWriteRepository>();
        using var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        Check.That(this.GetDb<Langue>(serviceProvider).Count()).IsEqualTo(2);

        var langue = new Langue
        {
            Code = "test", Libelle = "A001", Id = 1.ToGuid()
        };
        repository.Insert(langue);
        var nb = unitOfWork.SaveChanges();

        Check.That(nb).IsEqualTo(1);
        Check.That(this.GetDb<Langue>(serviceProvider).Count()).IsEqualTo(3);
    }

    [TestMethod]
    public void Query_Ok()
    {
        using var serviceProvider = CreateServiceCollection(GetServices);
        var repository = serviceProvider.GetRequiredService<IGenericWriteRepository>();

        var fromSeed = this.GetDb<Langue>(serviceProvider);
        Check.That(fromSeed.Count()).IsEqualTo(2);

        var fromBdd = repository.Query<Langue>().ToList();

        Check.That(fromBdd.Count()).IsEqualTo(2);
        Check.That(fromBdd.Select(x => x.Code)).IsOnlyMadeOf("fr", "en");
    }

    [TestMethod]
    public void Update_Ok()
    {
        using var serviceProvider = CreateServiceCollection(GetServices);
        var repository = serviceProvider.GetRequiredService<IGenericWriteRepository>();
        using var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        Check.That(this.GetDb<Langue>(serviceProvider).Count()).IsEqualTo(2);

        var toUpdate = this.GetDb<Langue>(serviceProvider).First();

        toUpdate.Code = "test";

        repository.Update(toUpdate);
        var nb = unitOfWork.SaveChanges();

        Check.That(nb).IsEqualTo(1);
        Check.That(this.GetDb<Langue>(serviceProvider).Count()).IsEqualTo(2);

        var fromBdd = this.GetDb<Langue>(serviceProvider).FirstOrDefault(x => x.Code == "test");
        Check.That(fromBdd).IsNotNull();
    }
}