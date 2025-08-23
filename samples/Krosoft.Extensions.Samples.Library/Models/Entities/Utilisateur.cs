using Krosoft.Extensions.Core.Helpers;
using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Entities;

public record Utilisateur : TenantEntity
{
    public string? Email { get; set; }
    public string? Nom { get; set; }
    public string? Titre { get; set; }
    public byte[]? Hash { get; set; }
    public byte[]? Salt { get; set; }
    public string? SecurityStamp { get; set; }
    public string? VerificationToken { get; set; }
    public Guid RoleId { get; set; }
    public Guid LangueId { get; set; }

    public int AccessFailedCount { get; set; }
    public DateTime? LockoutEndDate { get; set; }
    public DateTime? ConnexionDate { get; set; }
    public Guid Id { get; set; }
    public StatutCode StatutCode { get; set; }
    public Guid? ProprietaireId { get; set; }

    public void Clean()
    {
        Nom = StringHelper.TrimIfNotNull(Nom);
        Email = StringHelper.TrimIfNotNull(Email);
        Titre = StringHelper.TrimIfNotNull(Titre);
    }

    public string GetInitials() => StringHelper.GetAbbreviation(Nom);
}