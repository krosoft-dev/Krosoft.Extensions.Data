using Krosoft.Extensions.Samples.DotNet9.Api.Features.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Extensions;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features;

internal class DocumentsEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) =>
        app.MapGroup("/Documents")
           .DisableAntiforgery()
           .WithTags("Documents");

    public void Register(RouteGroupBuilder group)
    {
        group.MapPost("/Deposer/Fichier", async ([AsParameters] DeposerFichierDto dto,
                                                 IMediator mediator,
                                                 CancellationToken cancellationToken) =>
        {
            var file = await dto.File.ToFileAsync(cancellationToken);
            return await mediator.Send(new DeposerFichierCommand(dto.FichierId,
                                                                 file),
                                       cancellationToken);
        });
    }
}