using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;

public record Produit : Entity
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public ProduitMetadata? Metadata { get; set; }
}