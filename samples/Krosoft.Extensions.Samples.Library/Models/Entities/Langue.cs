using JetBrains.Annotations;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

[NoReorder]
public record Langue : Entity, IId
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Libelle { get; set; }
    public bool IsDefaut { get; set; }
    public bool IsActif { get; set; }
}