using System;
using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record Logiciel : TenantAuditableEntity 
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
    public Guid CategorieId { get; set; }
    public Categorie? Categorie { get; set; }
    public StatutCode StatutCode { get; set; }
}