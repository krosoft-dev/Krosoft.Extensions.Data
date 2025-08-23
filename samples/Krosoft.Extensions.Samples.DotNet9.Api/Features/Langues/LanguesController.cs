using Krosoft.Extensions.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Langues;

[AllowAnonymous]
public class LanguesController : ApiControllerBase
{
    [ProducesResponseType(typeof(IEnumerable<LangueDto>), StatusCodes.Status200OK)]
    [HttpGet]
    public Task<IEnumerable<LangueDto>> GetAsync([FromQuery] LanguesQuery query,
                                                 CancellationToken cancellationToken)
        => Mediator.Send(query, cancellationToken);
}