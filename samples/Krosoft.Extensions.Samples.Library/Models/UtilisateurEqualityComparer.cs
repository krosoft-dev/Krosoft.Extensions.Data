namespace Krosoft.Extensions.Samples.Library.Models;

public class UtilisateurEqualityComparer : IEqualityComparer<UtilisateurBasique>
{
    public bool Equals(UtilisateurBasique? x, UtilisateurBasique? y) => y != null && x != null && x.Nom == y.Nom && x.Prenom == y.Prenom;

    /// <summary>
    /// Retourne un code de hachage pour l'objet spécifié.
    /// </summary>
    /// <returns>
    /// Code de hachage pour l'objet spécifié.
    /// </returns>
    /// <param name="obj"><see cref="T:System.Object" /> pour lequel un code de hachage doit être retourné.</param>
    public int GetHashCode(UtilisateurBasique obj) => base.GetHashCode();
}