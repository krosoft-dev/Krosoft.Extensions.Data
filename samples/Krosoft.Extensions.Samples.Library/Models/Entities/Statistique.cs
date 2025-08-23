namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record Statistique : TenantEntity
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public int Nombre { get; set; }
}