using Krosoft.Extensions.Cqrs.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Update;

public record UpdateProduitCommand : BaseCommand<Unit>
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
}
