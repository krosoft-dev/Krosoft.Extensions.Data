namespace Krosoft.Extensions.Samples.Library.Models;

public record Item
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Libelle { get; set; }
    public string? Description { get; set; }
    public bool IsActif { get; set; }
    public DateTime Date { get; set; }
    public int? ValeurInt { get; set; }
    public decimal? ValeurDecimal { get; set; }
}