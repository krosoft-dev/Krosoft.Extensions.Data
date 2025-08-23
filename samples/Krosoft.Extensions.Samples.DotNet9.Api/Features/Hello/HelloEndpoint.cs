using Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.Create;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.Get;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello;

internal class HelloEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) => app.MapGroup("/Hello")
                                                                   .DisableAntiforgery()
                                                                   .WithTags("Hello");

    public void Register(RouteGroupBuilder group)
    {
        group.MapGet("/", (IMediator mediator,
                           CancellationToken cancellationToken)
                         => mediator.Send(new HelloQuery(), cancellationToken));

        group.MapPost("/", (HelloDotNet9CommandDto dto,
                            IMediator mediator,
                            CancellationToken cancellationToken)
                          => mediator.Send(new HelloCommand(dto.Name), cancellationToken));
    }
}