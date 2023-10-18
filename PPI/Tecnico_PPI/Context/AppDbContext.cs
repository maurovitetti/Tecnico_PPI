using Microsoft.EntityFrameworkCore;
using System.Reflection;
using PPI.SeedData;
using PPI.Models;

namespace PPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedingInicial.Seed(modelBuilder);
        }
        public DbSet<Accion> Acciones { get; set; }
        public DbSet<EstadoOrden> EstadoOrdenes { get; set; }
        public DbSet<OrdenInversion> OrdenInversiones { get; set; }
        public DbSet<TipoActivo> TipoActivos { get; set; }
        public DbSet<FCI> FCIs { get; set; }
        public DbSet<Bono> Bonos { get; set; }
    }
}
