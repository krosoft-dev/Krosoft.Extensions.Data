using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.GetById;

public record GetProduitByIdQuery(Guid Id) : BaseQuery<ProduitDto>;
