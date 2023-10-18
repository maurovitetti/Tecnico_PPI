using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPI.Models;

public class EstadoOrdenConfiguration : IEntityTypeConfiguration<EstadoOrden>
{
    public void Configure(EntityTypeBuilder<EstadoOrden> builder)
    {
        builder.ToTable("EstadoOrdenes");
        builder.HasKey(a => a.IdEstadoOrden)
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        builder.Property(a => a.DescripcionEstado)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(a => a.DescripcionEstado)
            .IsUnique();
    }
}
