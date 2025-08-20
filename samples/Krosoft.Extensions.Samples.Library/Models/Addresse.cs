using System.ComponentModel;

namespace Krosoft.Extensions.Samples.Library.Models;

public record Addresse
{
    public Addresse(string ligne1, string ligne2, string ville, string codePostal)
    {
        Ligne1 = ligne1;
        Ligne2 = ligne2;
        Ville = ville;
        CodePostal = codePostal;
    }

    [Description("Ligne1")]
    public string Ligne1 { get; }

    public string Ligne2 { get; }
    public string Ville { get; }
    public string CodePostal { get; }
}