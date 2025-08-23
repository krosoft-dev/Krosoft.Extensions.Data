using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record Logiciel : TenantEntity
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
    public Guid CategorieId { get; set; }
    public Categorie Categorie { get; set; } = null!;
    public StatutCode StatutCode { get; set; }
    public DateTime DateCreation { get; set; }
}