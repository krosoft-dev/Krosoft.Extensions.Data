using System.Net;
using Krosoft.Extensions.Core.Extensions;
using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Functional;

[TestClass]
public class LogicielsEndpointTests : SampleBaseApiTest<Program>
{
  
 

    [TestMethod]
    public async Task Logiciels_Ok()
    {
        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync("/Logiciels");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var result = await response.Content.ReadAsNewtonsoftJsonAsync<PaginationResult<LogicielDto>>(CancellationToken.None);
        Check.That(result).IsNotNull();
        Check.That(result?.Items).HasSize(7);
        Check.That(result?.Items.Select(x => x.Id.ToString()))
             .IsOnlyMadeOf("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c", 
                           "4560f48d-4d86-4b1e-8b22-51c1a9e3d6ca",
                           "1f39c60d-4f92-4f17-aa45-99e8f86a3b3a",
                           "3e8f94fd-03ea-47b0-b8c5-20b14c361fce",
                           "6c8a012b-cc8a-4da0-85a5-c9a87bb1a20a",
                           "9b5ccfd1-8c3e-4a0c-b4af-543a6f87042d",
                           "c40e3ff1-4f3a-4a3a-8b0c-c52a45e6e7b6");
    }

    [TestMethod]
    public async Task Logiciels_Text_Ok()
    {
        const string nom = "Excel";

        var httpClient = Factory.CreateClient();
        var response = await httpClient.GetAsync($"/Logiciels?Text={nom}");

        Check.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        var result = await response.Content.ReadAsNewtonsoftJsonAsync<PaginationResult<LogicielDto>>(CancellationToken.None);
        Check.That(result).IsNotNull();
        Check.That(result?.Items).HasSize(1);
        Check.That(result?.Items.Select(x => x.Nom))
             .IsOnlyMadeOf("Microsoft Excel");
    }
}