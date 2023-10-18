using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PPI.Models;

public class OrdenInversionConfiguration : IEntityTypeConfiguration<OrdenInversion>
{
    public void Configure(EntityTypeBuilder<OrdenInversion> builder)
    {
        builder.ToTable("OrdenInversiones");
        builder.HasKey(a => a.IdOrdenInversion)
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        builder.Property(a => a.IdCuenta).IsRequired();
        builder.Property(a => a.Cantidad).IsRequired();
        builder.Property(a => a.Operacion).IsRequired().HasMaxLength(1);
        builder.Property(a => a.PrecioUnitario).HasPrecision(18, 2).IsRequired(false);
        builder.Property(a => a.MontoTotal).HasPrecision(18, 2);

        builder.HasOne(a => a.EstadoOrden)
           .WithMany()
           .HasForeignKey(a => a.IdEstado);

        builder.HasOne(a => a.TipoActivo)
       .WithMany()
       .HasForeignKey(a => a.IdTipoActivo);

        builder.HasOne(a => a.FCI)
            .WithMany()
            .HasForeignKey(a => a.IdActivo);

        builder.HasOne(a => a.Bono)
            .WithMany()
            .HasForeignKey(a => a.IdActivo);

        builder.HasOne(a => a.Accion)
            .WithMany()
            .HasForeignKey(a => a.IdActivo);
    }
}
