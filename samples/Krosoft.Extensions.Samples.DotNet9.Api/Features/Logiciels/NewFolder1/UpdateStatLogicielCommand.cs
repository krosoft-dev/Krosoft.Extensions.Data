using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.NewFolder1;

public record UpdateStatLogicielCommand : PayloadBaseCommand
{
    public UpdateStatLogicielCommand(string payload) : base(payload)
    {
    }
}