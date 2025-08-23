using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public record LogicielDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
    public StatutCode StatutCode { get; set; }
    public DateTimeOffset CreationDate { get; set; }
}