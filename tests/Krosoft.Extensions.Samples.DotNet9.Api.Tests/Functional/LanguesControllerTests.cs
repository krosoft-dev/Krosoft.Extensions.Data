using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Langues;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Functional;

[TestClass]
public class LanguesEndpointTests : SampleBaseApiTest<Program>
{
    [TestMethod]
    public async Task Get_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Langues");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var langues = await response.Content.ReadAsNewtonsoftJsonAsync<IEnumerable<LangueDto>>(CancellationToken.None).ToList();
        Check.That(langues).IsNotNull();
        Check.That(langues).HasSize(2);
        Check.That(langues.Select(x => x.Code)).ContainsExactly("fr", "en");
    }
}