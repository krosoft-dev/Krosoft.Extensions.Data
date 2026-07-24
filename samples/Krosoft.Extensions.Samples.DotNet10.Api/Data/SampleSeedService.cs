using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Services;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Data;

public class SampleSeedService<T> : SeedService<T> where T : KrosoftContext;