using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.CreateBulk;

public record LogicielCreateBulkCommand : BaseCommand
{
    public int GroupeCount { get; set; } 
    public int LogicielCount { get; set; } 
}
 