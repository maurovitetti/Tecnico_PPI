using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPI.Models;

public class AccionConfiguration : IEntityTypeConfiguration<Accion>
{
    public void Configure(EntityTypeBuilder<Accion> builder)
    {
        builder.ToTable("Acciones");
        builder.HasKey(a => a.IdAccion)
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        builder.Property(a => a.Ticker).IsRequired().HasMaxLength(10);
        builder.Property(a => a.Nombre).IsRequired().HasMaxLength(50);
        builder.Property(a => a.PrecioUnitario).IsRequired().HasPrecision(18, 2);
    }
}

