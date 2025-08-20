using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Dto;

public record LogicielDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
    public string? Description { get; set; }
    public StatutCode StatutCode { get; set; }
    public DateTime DateCreation { get; set; }
}