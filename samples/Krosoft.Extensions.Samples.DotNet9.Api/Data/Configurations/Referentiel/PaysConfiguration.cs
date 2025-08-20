using Krosoft.Extensions.Data.EntityFramework.Configurations;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data.Configurations.Referentiel;

public class PaysConfiguration : ReferentielEntityTypeConfiguration<Pays>
{
    public PaysConfiguration() : base("Pays")
    {
    }

    protected override void ConfigureMore(EntityTypeBuilder<Pays> builder)
    {
        // Primary Key.
        builder.HasKey(t => t.Id);

        // Properties. 
        builder.Property(t => t.Code).IsRequired();
    }
}