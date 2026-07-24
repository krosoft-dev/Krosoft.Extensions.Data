using Krosoft.Extensions.Data.EntityFramework.Configurations;
using Krosoft.Extensions.Data.EntityFramework.Extensions;
using Krosoft.Extensions.Samples.DotNet10.Api.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Samples.DotNet10.Api.Data.Configurations;

public class ProduitConfiguration : ActiviteEntityTypeConfiguration<Produit>
{
    public ProduitConfiguration() : base("Produits")
    {
    }

    protected override void ConfigureMore(EntityTypeBuilder<Produit> builder)
    {
        // Primary Key.
        builder.HasKey(t => t.Id);

        // Properties.
        builder.Property(t => t.Nom).IsRequired();
        builder.Property(t => t.Metadata).HasJson();
    }
}