using AutoMapper;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.Abstractions.Extensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.GetAll;

public class ProduitsQueryHandler : IRequestHandler<ProduitsQuery, PaginationResult<ProduitDto>>
{
    private readonly ILogger<ProduitsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IReadRepository<Produit> _repository;

    public ProduitsQueryHandler(ILogger<ProduitsQueryHandler> logger,
                                IMapper mapper,
                                IReadRepository<Produit> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginationResult<ProduitDto>> Handle(ProduitsQuery request,
                                                           CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des produits...");

        var result = await _repository.Query()
                                      .Search(request.PaginationRequest.Text, x => x.Nom)
                                      .ToPaginationAsync<Produit, ProduitDto>(request.PaginationRequest,
                                                                              _mapper.ConfigurationProvider,
                                                                              cancellationToken);

        return result;
    }
}