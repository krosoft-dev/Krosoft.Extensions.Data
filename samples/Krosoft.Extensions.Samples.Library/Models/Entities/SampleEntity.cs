using Krosoft.Extensions.Data.Abstractions.Models;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record SampleEntity : Entity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}