using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Context;

public class DbVentasContext : DbContext
{
    public DbVentasContext(DbContextOptions<DbVentasContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbVentasContext).Assembly);
    }
}
