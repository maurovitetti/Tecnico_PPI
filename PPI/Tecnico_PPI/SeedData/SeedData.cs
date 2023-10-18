using Azure;
using Microsoft.EntityFrameworkCore;
using PPI.Models;

namespace PPI.SeedData
{
    public class SeedingInicial
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var acciones = new List<Accion>
            {
                new Accion
                {
                    IdAccion = 1,
                    Ticker = "AAPL",
                    Nombre = "Apple",
                    PrecioUnitario = 177.97M
                },
                new Accion
                {
                    IdAccion = 2,
                    Ticker = "GOOGL",
                    Nombre = "Alphabet Inc",
                    PrecioUnitario = 138.21M
                },
                new Accion
                {
                    IdAccion = 3,
                    Ticker = "MSFT",
                    Nombre = "Microsoft",
                    PrecioUnitario = 329.04M
                },
                new Accion
                {
                    IdAccion = 4,
                    Ticker = "KO",
                    Nombre = "Coca Cola",
                    PrecioUnitario = 58.3M
                },
                new Accion
                {
                    IdAccion = 5,
                    Ticker = "WMT",
                    Nombre = "Walmart",
                    PrecioUnitario = 163.42M
                }
            };

            modelBuilder.Entity<Accion>().HasData(acciones);

            var bonos = new List<Bono>
            {
                new Bono
                {
                    IdBono = 1,
                    Ticker = "Bono1",
                    Nombre = "Bono ejemplo1"
                },
                new Bono
                {
                    IdBono = 2,
                    Ticker = "Bono2",
                    Nombre = "Bono ejemplo2"
                },
                new Bono
                {
                    IdBono = 3,
                    Ticker = "Bono2",
                    Nombre = "Bono ejemplo3"
                }
            };

            modelBuilder.Entity<Bono>().HasData(bonos);

            var fcis = new List<FCI>
            {
                new FCI
                {
                    IdFCI = 1,
                    Ticker = "FCI1",
                    Nombre = "FCI ejemplo1"
                },
                new FCI
                {
                    IdFCI = 2,
                    Ticker = "FCI2",
                    Nombre = "FCI ejemplo2"
                },
                new FCI
                {
                    IdFCI = 3,
                    Ticker = "FCI3",
                    Nombre = "FCI ejemplo3"
                }
            };

            modelBuilder.Entity<FCI>().HasData(fcis);

            var TipoActivos = new List<TipoActivo>
            {
                new TipoActivo
                {
                    IdTipoActivo = 1,
                    Nombre = "FCI",
                    Comision = 0,
                    Impuesto = 0
                },
                new TipoActivo
                {
                    IdTipoActivo = 2,
                    Nombre = "Accion",
                    Comision = 0.6M,
                    Impuesto = 21
                },
                new TipoActivo
                {
                    IdTipoActivo = 3,
                    Nombre = "Bono",
                    Comision = 0.2M,
                    Impuesto =21
                }
            };

            modelBuilder.Entity<TipoActivo>().HasData(TipoActivos);

            var estadosOrden = new List<EstadoOrden>
            {
                new EstadoOrden
                {
                    IdEstadoOrden = 1,
                    DescripcionEstado = "En proceso"
                },
                new EstadoOrden
                {
                    IdEstadoOrden = 2,
                    DescripcionEstado = "Ejecutada"
                },
                new EstadoOrden
                {
                    IdEstadoOrden = 3,
                    DescripcionEstado = "Cancelada"
                }
            };

            modelBuilder.Entity<EstadoOrden>().HasData(estadosOrden);
        }
    }
}
