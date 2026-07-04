using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(e => e.CustomerID);
        builder.Property(e => e.CustomerID).ValueGeneratedOnAdd();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).HasColumnType("varchar(100)");
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.Phone).HasColumnType("varchar(20)");
        builder.Property(e => e.CityID).IsRequired();
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasOne(e => e.City)
            .WithMany(c => c.Customers)
            .HasForeignKey(e => e.CityID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
