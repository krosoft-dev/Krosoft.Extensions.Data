using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Core.Interfaces;
using MediatR;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Documents.DeposerFichier;

internal class DeposerFichierCommandHandler : IRequestHandler<DeposerFichierCommand, DepotDto>
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<DeposerFichierCommandHandler> _logger;

    public DeposerFichierCommandHandler(ILogger<DeposerFichierCommandHandler> logger,
                                        IDateTimeService dateTimeService)
    {
        _logger = logger;
        _dateTimeService = dateTimeService;
    }

    public async Task<DepotDto> Handle(DeposerFichierCommand request,
                                       CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Dépot du fichier '{request.File!.Name}' ({request.File.Content.Length}) avec l'identifiant '{request.FichierId}'...");

        var now = _dateTimeService.Now;

        await Task.CompletedTask;

        var tempPath = Path.Join("temp", request.FichierId.ToString());
        DirectoryHelper.CreateDirectoryIfNotExist(tempPath);

        using var xmlStream = new MemoryStream(request.File.Content);
        var filePath = Path.Join(tempPath, $"{now.Ticks}.txt");
        FileHelper.CreateFile(filePath, xmlStream);

        return new DepotDto
        {
            Message = $"Fichier créé sur {filePath}"
        };
    }
}