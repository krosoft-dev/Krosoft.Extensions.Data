using AutoMapper;
using Krosoft.Extensions.Core.Models;
using Krosoft.Extensions.Data.Abstractions.Extensions;
using Krosoft.Extensions.Data.Abstractions.Interfaces;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels._;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Logiciels.Logiciels;

public class LogicielsQueryHandler : IRequestHandler<LogicielsQuery, PaginationResult<LogicielDto>>
{
    private readonly ILogger<LogicielsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IReadRepository<Logiciel> _repository;

    public LogicielsQueryHandler(ILogger<LogicielsQueryHandler> logger,
                                 IMapper mapper,
                                 IReadRepository<Logiciel> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<PaginationResult<LogicielDto>> Handle(LogicielsQuery request,
                                                            CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des logiciels...");

        await Task.Delay(2000, cancellationToken);

        var result = await _repository.Query()
                                      .Search(request.Text, x => x.Nom)
                                      .ToPaginationAsync<Logiciel, LogicielDto>(request,
                                                                                _mapper.ConfigurationProvider,
                                                                                cancellationToken);

        return result;
    }
}