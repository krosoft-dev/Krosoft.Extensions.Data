namespace Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;

public record ProduitMetadata
{
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}