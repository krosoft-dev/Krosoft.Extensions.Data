using Bogus;
using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.CreateBulk;

internal class LogicielCreateBulkCommandHandler : IRequestHandler<LogicielCreateBulkCommand>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<LogicielCreateBulkCommandHandler> _logger;
    private readonly IWriteRepository<Categorie> _repositoryCategorie;
    private readonly IWriteRepository<Groupe> _repositoryGroupe;
    private readonly IWriteRepository<Logiciel> _repositoryLogiciel;
    private readonly IUnitOfWork _unitOfWork;

    public LogicielCreateBulkCommandHandler(ILogger<LogicielCreateBulkCommandHandler> logger,
                                            IDateTimeService dateTimeService,
                                            IWriteRepository<Logiciel> repositoryLogiciel,
                                            IUnitOfWork unitOfWork,
                                            IWriteRepository<Groupe> repositoryGroupe,
                                            IWriteRepository<Categorie> repositoryCategorie)
    {
        _logger = logger;
        _dateTimeService = dateTimeService;
        _repositoryLogiciel = repositoryLogiciel;
        _unitOfWork = unitOfWork;
        _repositoryGroupe = repositoryGroupe;
        _repositoryCategorie = repositoryCategorie;
    }

    public async Task Handle(LogicielCreateBulkCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création de logiciels en masses...");

        var categorieId = await _repositoryCategorie.Query()
                                                    .Select(x => x.Id)
                                                    .FirstOrDefaultAsync(cancellationToken
                                                                        );

        var fakerGroupe = new Faker<Groupe>()
                          .RuleFor(o => o.Id, _ => Guid.CreateVersion7())
                          .RuleFor(u => u.Name, (f, _) => f.Company.CompanyName())
                          .RuleFor(u => u.UpdatedAt, (f, _) => _dateTimeService.Now)
                          .RuleFor(u => u.CreatedAt, (f, _) => _dateTimeService.Now)
            ;

        var groupes = fakerGroupe.Generate(request.GroupeCount);
        var groupesId = groupes.Select(x => x.Id);

        var faker = new Faker<Logiciel>()
                    .RuleFor(o => o.Id, _ => Guid.CreateVersion7())
                    .RuleFor(u => u.Nom, (f, _) => f.Company.CompanyName())
                    .RuleFor(u => u.Description, (f, _) => f.Company.CatchPhrase())
                    .RuleFor(u => u.StatutCode, (f, _) => StatutCode.Public)
                    .RuleFor(u => u.GroupeId, (f, _) => f.PickRandom(groupesId))
                    .RuleFor(u => u.CategorieId, (f, _) => categorieId);

        var logiciels = faker.Generate(request.GroupeCount);

        _repositoryGroupe.InsertRange(groupes);
        _repositoryLogiciel.InsertRange(logiciels);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}