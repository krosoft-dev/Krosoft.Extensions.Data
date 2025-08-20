using Krosoft.Extensions.Samples.Library.Models.Dto;
using Krosoft.Extensions.Samples.Library.Models.Queries;
using Krosoft.Extensions.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features;

[AllowAnonymous]
public class PaysController : ApiControllerBase
{
    [ProducesResponseType(typeof(IEnumerable<PaysDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public Task<IEnumerable<PaysDto>> GetAsync([FromQuery] PaysQuery query,
                                               CancellationToken cancellationToken)
        => Mediator.Send(query, cancellationToken);
}