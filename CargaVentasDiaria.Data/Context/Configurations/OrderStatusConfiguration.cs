using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("OrderStatus");
        builder.HasKey(e => e.StatusID);
        builder.Property(e => e.StatusID).ValueGeneratedOnAdd();
        builder.Property(e => e.StatusName).IsRequired().HasMaxLength(30);
    }
}
