using Krosoft.Extensions.Cqrs.Models.Commands;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;

public abstract record LogicielBaseCommand<TReturn> : BaseCommand<TReturn>
{
    public string? Nom { get; set; }
    public Guid CategorieId { get; set; }
}