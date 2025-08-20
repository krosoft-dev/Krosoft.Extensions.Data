namespace Krosoft.Extensions.Samples.DotNet9.Api.Features.Models;

internal record AppSettings
{
    public string BaseUrl { get; set; } = null!;
    public string Token { get; set; } = null!;

    public IEnumerable<JobAmqpSettings> JobsAmqp { get; set; } = new List<JobAmqpSettings>();
}