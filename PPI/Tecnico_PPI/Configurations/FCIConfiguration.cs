using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPI.Models;

public class FCIConfiguration : IEntityTypeConfiguration<FCI>
{
    public void Configure(EntityTypeBuilder<FCI> builder)
    {
        builder.ToTable("FCIs");
        builder.HasKey(a => a.IdFCI)
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        builder.Property(a => a.Ticker).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Nombre).IsRequired().HasMaxLength(50);

        builder.HasIndex(a => a.Nombre)
            .IsUnique();
    }
}

