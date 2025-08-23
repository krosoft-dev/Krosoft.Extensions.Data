using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public record LogicielsDeleteCommand : BaseCommand
{
    public LogicielsDeleteCommand()
    {
        Ids = new HashSet<Guid>();
    }

    public IReadOnlySet<Guid> Ids { get; set; }
}