using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Hello.Create;

public record HelloCommand(string Name) : BaseCommand<string>;