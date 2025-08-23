using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public record LogicielsExportZipQuery : BaseQuery<IFileStream>;