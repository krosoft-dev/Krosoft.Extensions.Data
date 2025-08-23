using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public record LogicielDetailQuery : BaseQuery<LogicielDetailDto>
{
    public LogicielDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}