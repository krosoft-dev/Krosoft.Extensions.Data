namespace Krosoft.Extensions.Samples.Library.Models.Dto;

public record CorsPolicyDto
{
    public ICollection<string> Origins { get; set; } = new List<string>();
}