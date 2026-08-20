using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Seed;

public record SeedProduitsCommand : BaseCommand<SeedProduitsResultDto>
{
    public int Count { get; set; }
    public int BatchSize { get; set; } = 10_000;
}
