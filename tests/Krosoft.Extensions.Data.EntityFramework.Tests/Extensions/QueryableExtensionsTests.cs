using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Data.EntityFramework.InMemory.Extensions;
using Krosoft.Extensions.Data.EntityFramework.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Models;
using Krosoft.Extensions.Data.EntityFramework.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.Testing;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Extensions;

[TestClass]
public class QueryableExtensionsTests : BaseTest
{
    [TestMethod]
    public async Task ToPaginationAsyncTest()
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

        var request = new LogicielsQuery
        {
            PageSize = 2
        };

        var result = await repository.Query()
                                     .ToPaginationAsync(request, CancellationToken.None);

        Check.That(result).IsNotNull();
        Check.That(result.Items).HasSize(2);
        Check.That(result.Items.Select(x => x.Nom)).ContainsExactly("Logiciel1", "Logiciel2");
    }
}