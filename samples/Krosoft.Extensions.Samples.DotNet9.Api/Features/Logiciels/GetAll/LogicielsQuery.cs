using Krosoft.Extensions.Cqrs.Models.Queries;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.GetAll;

public record LogicielsQuery : SearchPaginationBaseQuery<LogicielDto>;
 