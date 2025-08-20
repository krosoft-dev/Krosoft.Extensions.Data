using Krosoft.Extensions.Data.EntityFramework.Interfaces;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data;

public class SampleKrosoftContextSeedService : ISeedService<SampleKrosoftContext>
{
    public bool Initialized { get; set; }

    public void InitializeDbForTests(SampleKrosoftContext db)
    {
    }
}