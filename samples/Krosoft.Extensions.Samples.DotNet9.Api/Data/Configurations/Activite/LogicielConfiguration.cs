using Krosoft.Extensions.Data.EntityFramework.Configurations;
using Krosoft.Extensions.Samples.Library.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Data.Configurations.Activite;

public class LogicielConfiguration : ActiviteEntityTypeConfiguration<Logiciel>
{
    public LogicielConfiguration() : base("Logiciels")
    {
    }

    protected override void ConfigureMore(EntityTypeBuilder<Logiciel> builder)
    {
        // Primary Key
        builder.HasKey(t => t.Id);

        // Properties.
        builder.Property(t => t.Nom).IsRequired();

        //Relationships. 
        builder.HasOne(e => e.Categorie)
               .WithMany(sl => sl.Logiciels)
               .HasForeignKey(e => e.CategorieId);
    }
}