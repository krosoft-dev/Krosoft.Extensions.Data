using Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.Get;
using Krosoft.Extensions.Samples.DotNet9.Api.Tests.Core;
using Krosoft.Extensions.Testing.Cqrs.Extensions;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Tests.Unit.Handlers.Queries;

[TestClass]
public class HelloWorlQueryHandlerTests : SampleBaseTest<Krosoft.Extensions.Samples.DotNet9.Api.Program>
{
    [TestMethod]
    public async Task Handle_Ok()
    {
        var serviceProvider = CreateServiceCollection();

        var result = await this.SendQueryAsync(serviceProvider, new HelloQuery());
        Check.That(result).IsNotNull();
        Check.That(result).IsEqualTo("Hello DotNet8");
    }
}