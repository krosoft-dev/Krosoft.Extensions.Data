using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Core.Models.Dto;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.Logiciels;
using Krosoft.Extensions.WebApi.Controllers;
using Krosoft.Extensions.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels;

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