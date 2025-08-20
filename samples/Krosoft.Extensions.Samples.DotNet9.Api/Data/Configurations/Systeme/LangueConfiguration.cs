using Krosoft.Extensions.Data.EntityFramework.Configurations;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data.Configurations.Systeme;

public class LangueConfiguration : SystemeEntityTypeConfiguration<Langue>
{
    public LangueConfiguration() : base("Langues")
    {
    }

    protected override void ConfigureMore(EntityTypeBuilder<Langue> builder)
    {
        // Primary Key
        builder.HasKey(t => t.Id);

        // Properties.
        builder.Property(t => t.Code).IsRequired();
        builder.Property(t => t.Libelle).IsRequired();
    }
}