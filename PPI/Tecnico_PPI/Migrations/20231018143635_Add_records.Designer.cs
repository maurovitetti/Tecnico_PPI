﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPI.Data;

#nullable disable

namespace PPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231018143635_Add_records")]
    partial class Add_records
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PPI.Models.Accion", b =>
                {
                    b.Property<int>("IdAccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAccion"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("PrecioUnitario")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("IdAccion")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.ToTable("Acciones", (string)null);

                    b.HasData(
                        new
                        {
                            IdAccion = 1,
                            Nombre = "Apple",
                            PrecioUnitario = 177.97m,
                            Ticker = "AAPL"
                        },
                        new
                        {
                            IdAccion = 2,
                            Nombre = "Alphabet Inc",
                            PrecioUnitario = 138.21m,
                            Ticker = "GOOGL"
                        },
                        new
                        {
                            IdAccion = 3,
                            Nombre = "Microsoft",
                            PrecioUnitario = 329.04m,
                            Ticker = "MSFT"
                        },
                        new
                        {
                            IdAccion = 4,
                            Nombre = "Coca Cola",
                            PrecioUnitario = 58.3m,
                            Ticker = "KO"
                        },
                        new
                        {
                            IdAccion = 5,
                            Nombre = "Walmart",
                            PrecioUnitario = 163.42m,
                            Ticker = "WMT"
                        });
                });

            modelBuilder.Entity("PPI.Models.Bono", b =>
                {
                    b.Property<int>("IdBono")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBono"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("IdBono")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("Bonos", (string)null);

                    b.HasData(
                        new
                        {
                            IdBono = 1,
                            Nombre = "Bono ejemplo1",
                            Ticker = "Bono1"
                        },
                        new
                        {
                            IdBono = 2,
                            Nombre = "Bono ejemplo2",
                            Ticker = "Bono2"
                        },
                        new
                        {
                            IdBono = 3,
                            Nombre = "Bono ejemplo3",
                            Ticker = "Bono2"
                        });
                });

            modelBuilder.Entity("PPI.Models.EstadoOrden", b =>
                {
                    b.Property<int>("IdEstadoOrden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEstadoOrden"));

                    b.Property<string>("DescripcionEstado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdEstadoOrden")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasIndex("DescripcionEstado")
                        .IsUnique();

                    b.ToTable("EstadoOrdenes", (string)null);

                    b.HasData(
                        new
                        {
                            IdEstadoOrden = 1,
                            DescripcionEstado = "En proceso"
                        },
                        new
                        {
                            IdEstadoOrden = 2,
                            DescripcionEstado = "Ejecutada"
                        },
                        new
                        {
                            IdEstadoOrden = 3,
                            DescripcionEstado = "Cancelada"
                        });
                });

            modelBuilder.Entity("PPI.Models.FCI", b =>
                {
                    b.Property<int>("IdFCI")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFCI"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("IdFCI")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("FCIs", (string)null);

                    b.HasData(
                        new
                        {
                            IdFCI = 1,
                            Nombre = "FCI ejemplo1",
                            Ticker = "FCI1"
                        },
                        new
                        {
                            IdFCI = 2,
                            Nombre = "FCI ejemplo2",
                            Ticker = "FCI2"
                        },
                        new
                        {
                            IdFCI = 3,
                            Nombre = "FCI ejemplo3",
                            Ticker = "FCI3"
                        });
                });

            modelBuilder.Entity("PPI.Models.OrdenInversion", b =>
                {
                    b.Property<int>("IdOrdenInversion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOrdenInversion"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("IdActivo")
                        .HasColumnType("int");

                    b.Property<int>("IdCuenta")
                        .HasColumnType("int");

                    b.Property<int>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int>("IdTipoActivo")
                        .HasColumnType("int");

                    b.Property<decimal>("MontoTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Operacion")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<decimal?>("PrecioUnitario")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdOrdenInversion")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasIndex("IdActivo");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdTipoActivo");

                    b.ToTable("OrdenInversiones", (string)null);
                });

            modelBuilder.Entity("PPI.Models.TipoActivo", b =>
                {
                    b.Property<int>("IdTipoActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTipoActivo"));

                    b.Property<decimal>("Comision")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Impuesto")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdTipoActivo")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.ToTable("TipoActivos", (string)null);

                    b.HasData(
                        new
                        {
                            IdTipoActivo = 1,
                            Comision = 0m,
                            Impuesto = 0m,
                            Nombre = "FCI"
                        },
                        new
                        {
                            IdTipoActivo = 2,
                            Comision = 0.6m,
                            Impuesto = 21m,
                            Nombre = "Accion"
                        },
                        new
                        {
                            IdTipoActivo = 3,
                            Comision = 0.2m,
                            Impuesto = 21m,
                            Nombre = "Bono"
                        });
                });

            modelBuilder.Entity("PPI.Models.OrdenInversion", b =>
                {
                    b.HasOne("PPI.Models.Accion", "Accion")
                        .WithMany()
                        .HasForeignKey("IdActivo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPI.Models.Bono", "Bono")
                        .WithMany()
                        .HasForeignKey("IdActivo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPI.Models.FCI", "FCI")
                        .WithMany()
                        .HasForeignKey("IdActivo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPI.Models.EstadoOrden", "EstadoOrden")
                        .WithMany()
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PPI.Models.TipoActivo", "TipoActivo")
                        .WithMany()
                        .HasForeignKey("IdTipoActivo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accion");

                    b.Navigation("Bono");

                    b.Navigation("EstadoOrden");

                    b.Navigation("FCI");

                    b.Navigation("TipoActivo");
                });
#pragma warning restore 612, 618
        }
    }
}
