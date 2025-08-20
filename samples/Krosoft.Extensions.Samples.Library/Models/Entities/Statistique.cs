using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record Statistique : TenantAuditableEntity
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public int Nombre { get; set; }
}