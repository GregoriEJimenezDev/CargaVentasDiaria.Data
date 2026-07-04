using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities");
        builder.HasKey(e => e.CityID);
        builder.Property(e => e.CityID).ValueGeneratedOnAdd();
        builder.Property(e => e.CityName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.CountryID).IsRequired();
        builder.HasOne(e => e.Country)
            .WithMany(c => c.Cities)
            .HasForeignKey(e => e.CountryID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
