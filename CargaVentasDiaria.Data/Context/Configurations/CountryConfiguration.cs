using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");
        builder.HasKey(e => e.CountryID);
        builder.Property(e => e.CountryID).ValueGeneratedOnAdd();
        builder.Property(e => e.CountryName).IsRequired().HasMaxLength(50);
    }
}
