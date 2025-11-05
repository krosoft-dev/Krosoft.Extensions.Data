using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.Delete;

internal class GroupeDeleteCommandHandler : IRequestHandler<GroupeDeleteCommand>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<GroupeDeleteCommandHandler> _logger;
    private readonly IWriteRepository<Groupe> _repository;
    private readonly IWriteRepository<Logiciel> _repositoryLogiciel;
    private readonly IUnitOfWork _unitOfWork;

    public GroupeDeleteCommandHandler(ILogger<GroupeDeleteCommandHandler> logger,
                                      IUnitOfWork unitOfWork,
                                      IWriteRepository<Groupe> repository,
                                      IWriteRepository<Logiciel> repositoryLogiciel,
                                      IDateTimeService dateTimeService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _repositoryLogiciel = repositoryLogiciel;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle(GroupeDeleteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Suppression du groupe '{GroupeId}'...", request.GroupeId);

        var now = _dateTimeService.Now;
        //await _repositoryLogiciel.Query()
        //                         .Where(x => x.GroupeId == request.GroupeId)
        //                         .ExecuteUpdateAsync(x => x.SetProperty(s => s.GroupeId, (Guid?)null)
        //                                                   .SetProperty(s => s.UpdatedAt, now),
        //                                             cancellationToken);

        await _repositoryLogiciel.UpdateRangeAsync(x => x.GroupeId == request.GroupeId,
                                                   x => x.SetProperty(s => s.GroupeId, null)
                                                         .SetProperty(s => s.UpdatedAt, now),
                                                   cancellationToken);

        await _repository.DeleteByIdAsync(request.GroupeId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}