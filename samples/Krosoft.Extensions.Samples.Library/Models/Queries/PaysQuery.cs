using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Samples.Library.Models.Dto;

namespace Krosoft.Extensions.Samples.Library.Models.Queries;

public record PaysQuery : BaseQuery<IEnumerable<PaysDto>>
{
    public string? Nom { get; set; }
}