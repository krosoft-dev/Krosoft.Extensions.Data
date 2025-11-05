
using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.Delete;

internal record GroupeDeleteCommand(Guid GroupeId) : BaseCommand;