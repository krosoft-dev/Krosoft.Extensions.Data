namespace Krosoft.Extensions.Data.Abstractions.Models;

public abstract record AuditableEntity : Entity, IAuditable
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}