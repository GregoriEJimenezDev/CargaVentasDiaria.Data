using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(e => e.ProductID);
        builder.Property(e => e.ProductID).ValueGeneratedOnAdd();
        builder.Property(e => e.ProductName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(e => e.Stock).IsRequired();
        builder.Property(e => e.CategoryID).IsRequired();
        builder.HasOne(e => e.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(e => e.CategoryID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
