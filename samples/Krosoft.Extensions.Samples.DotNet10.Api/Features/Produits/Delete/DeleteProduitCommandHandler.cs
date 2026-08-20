using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Models.Exceptions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Delete;

public class DeleteProduitCommandHandler : IRequestHandler<DeleteProduitCommand, Unit>
{
    private readonly ILogger<DeleteProduitCommandHandler> _logger;
    private readonly IWriteRepository<Produit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProduitCommandHandler(ILogger<DeleteProduitCommandHandler> logger,
                                       IWriteRepository<Produit> repository,
                                       IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProduitCommand request,
                                   CancellationToken cancellationToken)
    {
        _logger.LogInformation("Suppression du produit {Id}...", request.Id);

        var produit = await _repository.Query()
                                       .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (produit is null)
        {
            throw new EntityIntrouvableException<Produit>(request.Id);
        }

        _repository.Delete(produit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
