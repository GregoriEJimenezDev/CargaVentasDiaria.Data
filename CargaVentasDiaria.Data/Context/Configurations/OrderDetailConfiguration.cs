using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("Order_Details");
        builder.HasKey(e => e.DetailID);
        builder.Property(e => e.DetailID).ValueGeneratedOnAdd();
        builder.Property(e => e.OrderID).IsRequired();
        builder.Property(e => e.ProductID).IsRequired();
        builder.Property(e => e.Quantity).IsRequired();
        builder.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(e => e.TotalPrice).HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);
        builder.HasOne(e => e.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(e => e.OrderID)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(e => e.ProductID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
