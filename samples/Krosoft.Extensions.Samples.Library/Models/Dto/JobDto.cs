using Krosoft.Extensions.Samples.Library.Models.Enums;

namespace Krosoft.Extensions.Samples.Library.Models.Dto;

public record JobDto
{
    public string? Identifiant { get; set; }
    public string? CronExpression { get; set; }
    public JobTypeCode TypeCode { get; set; }
    public DateTime? ProchaineExecutionDate { get; set; }
    public string? DerniereExecutionStatut { get; set; }
    public DateTime? DerniereExecutionDate { get; set; }
    public DateTime? CreationDate { get; set; }
}