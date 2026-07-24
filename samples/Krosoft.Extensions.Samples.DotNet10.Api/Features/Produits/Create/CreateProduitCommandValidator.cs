using FluentValidation;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Create;

public class CreateProduitCommandValidator : AbstractValidator<CreateProduitCommand>
{
    public CreateProduitCommandValidator()
    {
        RuleFor(c => c.Nom).NotEmpty().NotNull();
    }
}
