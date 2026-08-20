using Krosoft.Extensions.Cqrs.Models.Queries;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.GetAll;

public record ProduitsQuery : SearchPaginationRequestQuery<ProduitDto>;

 