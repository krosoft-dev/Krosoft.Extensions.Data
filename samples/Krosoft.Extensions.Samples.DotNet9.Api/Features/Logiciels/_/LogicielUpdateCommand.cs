using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public record LogicielUpdateCommand : LogicielBaseCommand<Unit>
{
    public Guid Id { get; set; }
}