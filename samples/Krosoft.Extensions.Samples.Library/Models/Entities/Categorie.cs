using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.Abstractions.Models;
using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

[NoReorder]
public record Categorie : AuditableEntity, IId
{
    public Guid Id { get; set; }
    public string? Libelle { get; set; }
    public StatutCode StatutCode { get; set; }
    public IEnumerable<Logiciel> Logiciels { get; set; } = null!;
}