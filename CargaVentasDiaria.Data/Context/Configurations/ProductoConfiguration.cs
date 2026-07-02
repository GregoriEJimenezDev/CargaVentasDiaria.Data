using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
        builder.Property(e => e.CategoriaId).IsRequired();
        builder.HasIndex(e => e.Codigo).IsUnique();
        builder.HasOne(e => e.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(e => e.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
