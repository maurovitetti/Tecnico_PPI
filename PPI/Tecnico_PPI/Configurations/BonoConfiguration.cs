using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPI.Models;

public class BonoConfiguration : IEntityTypeConfiguration<Bono>
{
    public void Configure(EntityTypeBuilder<Bono> builder)
    {
        builder.ToTable("Bonos");
        builder.HasKey(a => a.IdBono)
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        builder.Property(a => a.Ticker).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Nombre).IsRequired().HasMaxLength(50);

        builder.HasIndex(a => a.Nombre)
            .IsUnique();
    }
}

