using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class ProductService : IProductService
{
    private readonly DbVentasContext _context;

    public ProductService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearAsync(string nombreProducto, int categoryId, decimal price, int stock)
    {
        try
        {
            var nombreTrim = nombreProducto.Trim().ToLowerInvariant();
            var producto = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductName.Trim().ToLower() == nombreTrim);

            if (producto != null)
                return OperationResult<int>.Ok(producto.ProductID);

            var nuevo = new Product
            {
                ProductName = nombreProducto.Trim(),
                CategoryID = categoryId,
                Price = price,
                Stock = stock
            };
            _context.Products.Add(nuevo);
            await _context.SaveChangesAsync();

            return OperationResult<int>.Ok(nuevo.ProductID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener producto: {ex.Message}");
        }
    }

    public async Task<OperationResult<int>> BuscarPorNombreAsync(string nombreProducto)
    {
        try
        {
            var nombreTrim = nombreProducto.Trim().ToLowerInvariant();
            var producto = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductName.Trim().ToLower() == nombreTrim);

            if (producto != null)
                return OperationResult<int>.Ok(producto.ProductID);

            return OperationResult<int>.Fail($"Producto '{nombreProducto}' no encontrado.");
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al buscar producto: {ex.Message}");
        }
    }
}
