using Krosoft.Extensions.Data.EntityFramework.Configurations;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data.Configurations.Activite;

internal class GroupeConfiguration() : ActiviteEntityTypeConfiguration<Groupe>("Groupes")
{
    protected override void ConfigureMore(EntityTypeBuilder<Groupe> builder)
    {
        // Primary Key.
        builder.HasKey(t => t.Id);

        // Properties.    
        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();

        builder.HasMany(tg => tg.Logiciels)
               .WithOne(t => t.Groupe)
               .HasForeignKey(t => t.GroupeId);
    }
}