using Krosoft.Extensions.Cqrs.Models.Commands;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Features.Produits.Delete;

public record DeleteProduitCommand(Guid Id) : BaseCommand<Unit>;
