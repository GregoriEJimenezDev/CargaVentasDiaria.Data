using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("Ventas");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Fecha).IsRequired();
        builder.Property(e => e.ClienteId).IsRequired();
        builder.Property(e => e.ProductoId).IsRequired();
        builder.Property(e => e.Cantidad).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.PrecioUnitario).IsRequired().HasColumnType("decimal(18,2)");
        builder.HasOne(e => e.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Producto)
            .WithMany(p => p.Ventas)
            .HasForeignKey(e => e.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
