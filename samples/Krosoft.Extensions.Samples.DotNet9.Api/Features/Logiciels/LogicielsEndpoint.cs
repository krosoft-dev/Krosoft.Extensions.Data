using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.NewFolder1;
using Krosoft.Extensions.Samples.Library.Models.Messages;
using Krosoft.Extensions.WebApi.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels;

internal class LogicielsEndpoint : IEndpoint
{
    public RouteGroupBuilder DefineGroup(WebApplication app) =>
        app.MapGroup("/Logiciels")
           .DisableAntiforgery()
           .WithTags("Logiciels");

    public void Register(RouteGroupBuilder group)
    {
        group.MapPost("/Update/Stats", (  string tenantId,
                                          string userId,
                                        IMediator mediator,
                                        CancellationToken cancellationToken) =>
        {
            var message = new UpdateStatLogicielMessage
            {
                TenantId = tenantId,
                UtilisateurId = userId
            };

            var payload = JsonConvert.SerializeObject(message);

            return mediator.Send(new UpdateStatLogicielCommand(payload),
                                 cancellationToken);
        });
    }
}