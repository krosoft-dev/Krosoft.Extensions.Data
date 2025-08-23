using Krosoft.Extensions.Core.Interfaces;
using Krosoft.Extensions.Core.Tools;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Data;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Krosoft.Extensions.Samples.Library.Models.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.NewFolder1;

internal class UpdateStatLogicielCommandHandler : IRequestHandler<UpdateStatLogicielCommand>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<UpdateStatLogicielCommandHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UpdateStatLogicielCommandHandler(ILogger<UpdateStatLogicielCommandHandler> logger,
                                            IServiceProvider serviceProvider,
                                            IDateTimeService dateTimeService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _dateTimeService = dateTimeService;
    }

    public async Task Handle(UpdateStatLogicielCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Mise à jour des statistiques...");

        if (!string.IsNullOrEmpty(request.Payload))
        {
            var message = JsonConvert.DeserializeObject<UpdateStatLogicielMessage>(request.Payload);
            if (message != null)
            {
                _logger.LogInformation($"Mise à jour des statistiques pour le tenant {message.TenantId}");

                using (var scope = _serviceProvider.CreateDbContextScope<SampleKrosoftTenantAuditableContext, string>(message.TenantId!,
                                                                                                                      _dateTimeService.Now,
                                                                                                                      message.UtilisateurId!))
                {
                    var repositoryLogiciel = scope.GetReadRepository<Logiciel>();
                    var nombre = await repositoryLogiciel.Query().CountAsync(cancellationToken);

                    var repositoryStatistique = scope.GetWriteRepository<Statistique>();
                    var statistique = await repositoryStatistique.Query()
                                                                 .FirstOrDefaultAsync(cancellationToken);
                    if (statistique == null)
                    {
                        statistique = new Statistique
                        {
                            Id = SequentialGuid.NewGuid(),
                            Nom = message.TenantId,
                            Nombre = nombre
                        };
                        repositoryStatistique.Insert(statistique);
                    }
                    else
                    {
                        statistique.Nombre = nombre;
                        repositoryStatistique.Update(statistique);
                    }

                    var unitOfWork = scope.GetUnitOfWork();
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    return;
                }
            }
        }

        _logger.LogError($"Impossible de mettre à jour les statitisques à partir du payload : {request.Payload}");
    }
}