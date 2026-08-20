using AutoMapper;
using AutoMapper.QueryableExtensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Models.Exceptions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.GetById;

public class GetProduitByIdQueryHandler : IRequestHandler<GetProduitByIdQuery, ProduitDto>
{
    private readonly ILogger<GetProduitByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IReadRepository<Produit> _repository;

    public GetProduitByIdQueryHandler(ILogger<GetProduitByIdQueryHandler> logger,
                                      IMapper mapper,
                                      IReadRepository<Produit> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ProduitDto> Handle(GetProduitByIdQuery request,
                                         CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération du produit {Id}...", request.Id);

        var dto = await _repository.Query()
                                   .Where(p => p.Id == request.Id)
                                   .ProjectTo<ProduitDto>(_mapper.ConfigurationProvider)
                                   .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
        {
            throw new EntityIntrouvableException<Produit>(request.Id);
        }

        return dto;
    }
}
