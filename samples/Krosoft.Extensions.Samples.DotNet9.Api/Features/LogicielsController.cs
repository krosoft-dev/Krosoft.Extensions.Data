using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Cqrs.Models.Commands;
using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Controllers;
using Krosoft.Extensions.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features;

[AllowAnonymous]
public class LogicielsController : ApiControllerBase
{
   
    [HttpPost]
    public Task<Guid> CreateAsync([FromBody] LogicielCreateCommand command, CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);
     
    [HttpDelete]
    public Task DeleteAsync([FromBody] LogicielsDeleteCommand command,
                            CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);

 

       [HttpGet]
    public Task<PaginationResult<LogicielDto>> GetAsync([FromQuery] LogicielsQuery query,
                                                        CancellationToken cancellationToken)
        => Mediator.Send(query, cancellationToken);

    [ProducesResponseType(typeof(LogicielDetailDto), StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public Task<LogicielDetailDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => Mediator.Send(new LogicielDetailQuery(id), cancellationToken);

    [ProducesResponseType(typeof(IEnumerable<PickListDto>), StatusCodes.Status200OK)]
    [HttpGet("PickList")]
    public Task<IEnumerable<PickListDto>> GetPickListAsync(CancellationToken cancellationToken)
        => Mediator.Send(new LogicielsPickListQuery(), cancellationToken);

    [HttpPost("Import")]
    public async Task<int> ImportAsync(CancellationToken cancellationToken)
    {
        var files = await this.GetRequestToBase64StringAsync();
        return await Mediator.Send(new LogicielImportCommand(files), cancellationToken);
    }

    [HttpPut]
    public Task UpdateAsync([FromBody] LogicielUpdateCommand command, CancellationToken cancellationToken)
        => Mediator.Send(command, cancellationToken);
}
public record LogicielImportCommand : AuthBaseCommand<int>
{
    public LogicielImportCommand(IEnumerable<string> files)
    {
        Files = files;
    }

    public IEnumerable<string> Files { get; }
}

public record LogicielCreateCommand : LogicielBaseCommand<Guid>;
 
public abstract record LogicielBaseCommand<TReturn> : BaseCommand<TReturn>
{
    public string? Nom { get; set; }
    public Guid CategorieId { get; set; }
}
 

public record LogicielUpdateCommand : LogicielBaseCommand<Unit>
{
    public Guid Id { get; set; }
}
 
public record LogicielsDeleteCommand : BaseCommand
{
    public LogicielsDeleteCommand()
    {
        Ids = new HashSet<Guid>();
    }

    public IReadOnlySet<Guid> Ids { get; set; }
}