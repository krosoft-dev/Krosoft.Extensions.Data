using Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.Delete;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.GetAll;
using Krosoft.Extensions.WebApi.Interfaces;
using Krosoft.Extensions.WebApi.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes;

internal class GroupesEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) =>
        app.MapGroup("/Groupes")
           .DisableAntiforgery()
           .WithTags("Groupes");

    public void Register(RouteGroupBuilder group)
    {
        group.MapGet("/", ([AsParameters] GroupesQuery query,
                           [AsParameters] PaginationDto paginationDto,
                           IMediator mediator,
                           CancellationToken cancellationToken)
                         => mediator.Send(query.SetPagination(paginationDto), cancellationToken));

        group.MapDelete("/{groupeId:guid}", ([FromRoute] Guid groupeId,
                                             IMediator mediator,
                                             CancellationToken cancellationToken)
                            => mediator.Send(new GroupeDeleteCommand(groupeId), cancellationToken));
    }
}