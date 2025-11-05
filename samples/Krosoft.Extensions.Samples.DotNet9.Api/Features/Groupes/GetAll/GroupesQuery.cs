using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.GetAll;

public record GroupesQuery : SearchPaginationRequestQuery<GroupeDto>;
 
 