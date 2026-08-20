namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits;

public record ProduitDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
}
