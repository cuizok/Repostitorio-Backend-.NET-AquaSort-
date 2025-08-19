using Microsoft.EntityFrameworkCore;
using AquaSort.Api.Models;

namespace AquaSort.Api.Data
{
    public class AquaSortContext : DbContext
    {
        public AquaSortContext(DbContextOptions<AquaSortContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Proveedor>().ToTable("Proveedor"); // nombre exacto de la tabla
}

    }
}
