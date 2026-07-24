using Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Create;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Delete;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.GetAll;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.GetById;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Seed;
using Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Update;
using Krosoft.Extensions.WebApi.Interfaces;
using Krosoft.Extensions.WebApi.Models.Dto;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits;

internal class ProduitsEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) =>
        app.MapGroup("/Produits")
           .DisableAntiforgery()
           .WithTags("Produits");

    public void Register(RouteGroupBuilder group)
    {
        group.MapGet("/", ([AsParameters] ProduitsQuery query,
                           [AsParameters] PaginationDto paginationDto,
                           IMediator mediator,
                           CancellationToken cancellationToken)
                         => mediator.Send(query.SetPagination(paginationDto), cancellationToken));

        group.MapGet("/{id:guid}", (Guid id,
                                    IMediator mediator,
                                    CancellationToken cancellationToken)
                                  => mediator.Send(new GetProduitByIdQuery(id), cancellationToken));

        group.MapPost("/", (CreateProduitCommandDto dto,
                            IMediator mediator,
                            CancellationToken cancellationToken)
                          => mediator.Send(new CreateProduitCommand
                          {
                              Nom = dto.Nom,
                              Description = dto.Description
                          }, cancellationToken));

        group.MapPost("/seed", (SeedProduitsCommandDto dto,
                                IMediator mediator,
                                CancellationToken cancellationToken)
                              => mediator.Send(new SeedProduitsCommand
                              {
                                  Count = dto.Count,
                                  BatchSize = dto.BatchSize ?? 10_000
                              }, cancellationToken));

        group.MapPut("/{id:guid}", (Guid id,
                                    UpdateProduitCommandDto dto,
                                    IMediator mediator,
                                    CancellationToken cancellationToken)
                                  => mediator.Send(new UpdateProduitCommand
                                  {
                                      Id = id,
                                      Nom = dto.Nom,
                                      Description = dto.Description
                                  }, cancellationToken));

        group.MapDelete("/{id:guid}", (Guid id,
                                       IMediator mediator,
                                       CancellationToken cancellationToken)
                                     => mediator.Send(new DeleteProduitCommand(id), cancellationToken));
    }
}
