using CargaVentasDiaria.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargaVentasDiaria.Data.Context.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
        builder.HasIndex(e => e.Codigo).IsUnique();
    }
}
