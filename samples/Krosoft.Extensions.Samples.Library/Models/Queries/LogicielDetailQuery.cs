using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Samples.Library.Models.Dto;

namespace Krosoft.Extensions.Samples.Library.Models.Queries;

public record LogicielDetailQuery : BaseQuery<LogicielDetailDto>
{
    public LogicielDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}