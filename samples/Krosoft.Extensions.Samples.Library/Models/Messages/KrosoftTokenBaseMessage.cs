namespace Krosoft.Extensions.Samples.Library.Models.Messages;

public abstract class KrosoftTokenBaseMessage
{
    public string? UtilisateurId { get; set; }
    public string? TenantId { get; set; }
}