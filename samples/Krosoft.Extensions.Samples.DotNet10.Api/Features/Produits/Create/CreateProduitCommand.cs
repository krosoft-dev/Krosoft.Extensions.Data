using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Create;

public record CreateProduitCommand : BaseCommand<Guid>
{
    public string? Nom { get; set; }
    public string? Description { get; set; }
}
