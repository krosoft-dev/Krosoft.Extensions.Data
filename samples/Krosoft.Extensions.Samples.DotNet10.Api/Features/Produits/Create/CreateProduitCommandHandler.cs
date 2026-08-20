using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Create;

public class CreateProduitCommandHandler : IRequestHandler<CreateProduitCommand, Guid>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<CreateProduitCommandHandler> _logger;
    private readonly IWriteRepository<Produit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProduitCommandHandler(ILogger<CreateProduitCommandHandler> logger,
                                       IWriteRepository<Produit> repository,
                                       IUnitOfWork unitOfWork,
                                       IDateTimeService dateTimeService)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
    }

    public async Task<Guid> Handle(CreateProduitCommand request,
                                   CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création du produit {Nom}...", request.Nom);

        var produit = new Produit
        {
            Id = Guid.NewGuid(),
            Nom = request.Nom,
            Metadata = new ProduitMetadata
            {
                Description = request.Description,
                CreatedAt = _dateTimeService.Now
            }
        };

        _repository.Insert(produit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return produit.Id;
    }
}
