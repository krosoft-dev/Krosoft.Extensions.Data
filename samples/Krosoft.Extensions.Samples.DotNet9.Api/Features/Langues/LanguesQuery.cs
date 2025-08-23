using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Langues;

public record LanguesQuery : BaseQuery<IEnumerable<LangueDto>>;