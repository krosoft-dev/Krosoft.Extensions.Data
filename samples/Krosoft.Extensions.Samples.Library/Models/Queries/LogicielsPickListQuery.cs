using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.Library.Models.Queries;

public record LogicielsPickListQuery : BaseQuery<IEnumerable<PickListDto>>;