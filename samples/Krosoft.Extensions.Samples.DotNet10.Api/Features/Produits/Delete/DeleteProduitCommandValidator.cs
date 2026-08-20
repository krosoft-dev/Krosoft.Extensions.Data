using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Delete;

public class DeleteProduitCommandValidator : AbstractValidator<DeleteProduitCommand>
{
    public DeleteProduitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
