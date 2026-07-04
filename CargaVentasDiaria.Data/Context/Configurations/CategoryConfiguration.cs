using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(e => e.CategoryID);
        builder.Property(e => e.CategoryID).ValueGeneratedOnAdd();
        builder.Property(e => e.CategoryName).IsRequired().HasMaxLength(50);
    }
}
