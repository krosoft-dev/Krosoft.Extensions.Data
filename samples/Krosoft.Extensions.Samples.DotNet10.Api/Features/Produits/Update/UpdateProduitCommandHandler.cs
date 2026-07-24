using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Models.Exceptions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Update;

public class UpdateProduitCommandHandler : IRequestHandler<UpdateProduitCommand, Unit>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<UpdateProduitCommandHandler> _logger;
    private readonly IWriteRepository<Produit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProduitCommandHandler(ILogger<UpdateProduitCommandHandler> logger,
                                       IWriteRepository<Produit> repository,
                                       IUnitOfWork unitOfWork,
                                       IDateTimeService dateTimeService)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
    }

    public async Task<Unit> Handle(UpdateProduitCommand request,
                                   CancellationToken cancellationToken)
    {
        _logger.LogInformation("Mise à jour du produit {Id}...", request.Id);

        var produit = await _repository.Query()
                                       .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (produit is null)
        {
            throw new EntityIntrouvableException<Produit>(request.Id);
        }

        produit.Nom = request.Nom;
        produit.Metadata = new ProduitMetadata
        {
            Description = request.Description,
            CreatedAt = produit.Metadata?.CreatedAt ?? _dateTimeService.Now,
            Tags = produit.Metadata?.Tags ?? new List<string>()
        };

        _repository.Update(produit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
