using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Cqrs.Models.Commands;
using Krosoft.Extensions.Samples.Library.Models.Dto;

namespace Krosoft.Extensions.Samples.Library.Models.Queries;

public record DeposerFichierCommand(
    long FichierId,
    KrosoftFile? File)
    : BaseCommand<DepotDto>;