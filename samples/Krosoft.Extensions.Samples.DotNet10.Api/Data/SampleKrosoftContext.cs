using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Data;

public class SampleKrosoftContext : KrosoftContext
{
    public SampleKrosoftContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDataJson<Produit>();
    }
}