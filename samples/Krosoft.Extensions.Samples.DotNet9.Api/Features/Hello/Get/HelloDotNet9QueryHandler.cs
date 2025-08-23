using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.Get;

public class HelloDotNet9QueryHandler : IRequestHandler<HelloQuery, string>
{
    private readonly ILogger<HelloDotNet9QueryHandler> _logger;

    public HelloDotNet9QueryHandler(ILogger<HelloDotNet9QueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloQuery request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello DotNet9...");

        return Task.FromResult("Hello DotNet9");
    }
}