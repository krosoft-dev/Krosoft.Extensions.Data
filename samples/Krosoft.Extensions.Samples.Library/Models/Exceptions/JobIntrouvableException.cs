using Krosoft.Extensions.Core.Models.Exceptions;

namespace Krosoft.Extensions.Samples.Library.Models.Exceptions;

public class JobIntrouvableException(string id) : KrosoftTechnicalException($"Job '{id}' introuvable.");