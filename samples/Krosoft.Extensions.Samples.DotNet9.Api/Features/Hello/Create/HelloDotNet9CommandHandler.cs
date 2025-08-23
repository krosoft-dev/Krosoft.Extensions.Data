using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.Create;

public class HelloDotNet9CommandHandler : IRequestHandler<HelloCommand, string>
{
    private readonly ILogger<HelloDotNet9CommandHandler> _logger;

    public HelloDotNet9CommandHandler(ILogger<HelloDotNet9CommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<string> Handle(HelloCommand request,
                               CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Hello {request.Name}...");

        return Task.FromResult($"Hello {request.Name} !");
    }
}