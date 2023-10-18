using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPI.Models;

public class TipoActivoConfiguration : IEntityTypeConfiguration<TipoActivo>
{
    public void Configure(EntityTypeBuilder<TipoActivo> builder)
    {
        builder.ToTable("TipoActivos");

        builder.HasKey(t => t.IdTipoActivo)
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        builder.Property(t => t.Nombre).IsRequired().HasMaxLength(50); ;
        builder.Property(t => t.Comision).HasPrecision(18, 2);
        builder.Property(t => t.Impuesto).HasPrecision(18, 2);
    }
}

