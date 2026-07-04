using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(e => e.OrderID);
        builder.Property(e => e.OrderID).ValueGeneratedOnAdd();
        builder.Property(e => e.CustomerID).IsRequired();
        builder.Property(e => e.StatusID).IsRequired();
        builder.Property(e => e.OrderDate).IsRequired().HasColumnType("datetime");
        builder.HasOne(e => e.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(e => e.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Status)
            .WithMany(s => s.Orders)
            .HasForeignKey(e => e.StatusID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
