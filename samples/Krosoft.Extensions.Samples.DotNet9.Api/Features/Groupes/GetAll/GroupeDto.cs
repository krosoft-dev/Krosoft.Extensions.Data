using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.GetAll;

public record GroupeDto
{
    public Guid Id { get; set; }
    public string? Nom { get; set; }
  
    public int LogicielsCount { get; set; }
 
    public DateTimeOffset CreationDate { get; set; }
  
}