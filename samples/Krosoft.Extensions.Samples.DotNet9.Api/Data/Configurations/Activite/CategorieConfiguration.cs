using Krosoft.Extensions.Data.EntityFramework.Configurations;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data.Configurations.Activite;

public class CategorieConfiguration : ActiviteEntityTypeConfiguration<Categorie>
{
    public CategorieConfiguration() : base("Categories")
    {
    }

    protected override void ConfigureMore(EntityTypeBuilder<Categorie> builder)
    {
        // Primary Key.
        builder.HasKey(t => t.Id);

        // Properties.
        builder.Property(t => t.Libelle).IsRequired();
        builder.Property(t => t.StatutCode).IsRequired();
    }
}