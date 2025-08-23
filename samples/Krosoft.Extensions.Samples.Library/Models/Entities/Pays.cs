using System;
using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record Pays : AuditableEntity
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
}