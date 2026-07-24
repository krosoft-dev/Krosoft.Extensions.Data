using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Seed;

public class SeedProduitsCommandHandler : IRequestHandler<SeedProduitsCommand, SeedProduitsResultDto>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<SeedProduitsCommandHandler> _logger;
    private readonly IWriteRepository<Produit> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SeedProduitsCommandHandler(ILogger<SeedProduitsCommandHandler> logger,
                                      IWriteRepository<Produit> repository,
                                      IUnitOfWork unitOfWork,
                                      IDateTimeService dateTimeService)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateTimeService = dateTimeService;
    }

    public async Task<SeedProduitsResultDto> Handle(SeedProduitsCommand request,
                                                    CancellationToken cancellationToken)
    {
        _logger.LogInformation("Génération de {Count} produits en masse...", request.Count);

        var batchSize = request.BatchSize > 0 ? request.BatchSize : 10000;
        var generatedCount = 0;

        while (generatedCount < request.Count && !cancellationToken.IsCancellationRequested)
        {
            var countToGenerate = Math.Min(batchSize, request.Count - generatedCount);
            var produits = new List<Produit>(countToGenerate);

            for (var i = 0; i < countToGenerate; i++)
            {
                var id = Guid.NewGuid();
                produits.Add(new Produit
                {
                    Id = id,
                    Nom = $"Produit {id}",
                    Metadata = new ProduitMetadata
                    {
                        Description = $"Description du produit {id}",
                        CreatedAt = _dateTimeService.Now
                    }
                });
            }

            _repository.InsertRange(produits);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            generatedCount += countToGenerate;
            _logger.LogInformation("{GeneratedCount}/{Count} produits générés...", generatedCount, request.Count);
        }

        return new SeedProduitsResultDto(generatedCount);
    }
}
