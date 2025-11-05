using AutoMapper;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.Abstractions.Extensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Groupes.GetAll;

public class GroupesQueryHandler : IRequestHandler<GroupesQuery, PaginationResult<GroupeDto>>
{
    private readonly ILogger<GroupesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IReadRepository<Groupe> _repository;

    public GroupesQueryHandler(ILogger<GroupesQueryHandler> logger,
                               IMapper mapper,
                               IReadRepository<Groupe> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginationResult<GroupeDto>> Handle(GroupesQuery request,
                                                          CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des groupes...");

        var paginationResult = await _repository.Query()
                                                .Search(request.PaginationRequest.Text,
                                                        x => x.Id.ToString(),
                                                        x => x.Name)
                                                .ToPaginationAsync<Groupe, GroupeDto>(request.PaginationRequest,
                                                                                      _mapper.ConfigurationProvider,
                                                                                      cancellationToken);

        _logger.LogInformation($"Récupération de {paginationResult.Items.Count()} groupes de tenants.");

        return paginationResult;
    }
}