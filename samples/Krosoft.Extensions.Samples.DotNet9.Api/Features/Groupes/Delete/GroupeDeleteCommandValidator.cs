using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.Delete;

internal class GroupeDeleteCommandValidator : AbstractValidator<GroupeDeleteCommand>
{
    public GroupeDeleteCommandValidator()
    {
        RuleFor(v => v.GroupeId)
            .NotEmpty();
    }
}