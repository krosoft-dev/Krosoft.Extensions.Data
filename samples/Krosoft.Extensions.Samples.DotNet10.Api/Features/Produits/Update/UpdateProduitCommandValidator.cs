using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Update;

public class UpdateProduitCommandValidator : AbstractValidator<UpdateProduitCommand>
{
    public UpdateProduitCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Nom).NotEmpty().NotNull();
    }
}
