using Krosoft.Extensions.Data.EntityFramework.Contexts;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data;

public class SampleKrosoftContext : KrosoftContext
{
    public SampleKrosoftContext(DbContextOptions<SampleKrosoftContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDataJson<Categorie>();
        modelBuilder.HasDataJson<Statistique>();
        modelBuilder.HasDataJson<Logiciel>();
        modelBuilder.HasDataJson<Langue>();
        modelBuilder.HasDataJson<Pays>();
    }
}