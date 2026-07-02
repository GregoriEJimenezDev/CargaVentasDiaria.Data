using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class ProductoService : IProductoService
{
    private readonly DbVentasContext _context;

    public ProductoService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> ObtenerOCrearAsync(string codigo, string nombre, int categoriaId)
    {
        try
        {
            var producto = await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Codigo == codigo);

            if (producto != null)
                return Result<int>.Ok(producto.Id);

            var nuevo = new Producto
            {
                Codigo = codigo,
                Nombre = nombre,
                CategoriaId = categoriaId
            };
            _context.Productos.Add(nuevo);
            await _context.SaveChangesAsync();

            return Result<int>.Ok(nuevo.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error al crear/obtener producto: {ex.Message}");
        }
    }
}
