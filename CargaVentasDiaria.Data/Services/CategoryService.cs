using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class CategoryService : ICategoryService
{
    private readonly DbVentasContext _context;

    public CategoryService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearAsync(string nombre)
    {
        try
        {
            var nombreTrim = nombre.Trim().ToLowerInvariant();
            var categoria = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryName.Trim().ToLower() == nombreTrim);

            if (categoria != null)
                return OperationResult<int>.Ok(categoria.CategoryID);

            var nueva = new Category { CategoryName = nombre.Trim() };
            _context.Categories.Add(nueva);
            await _context.SaveChangesAsync();

            return OperationResult<int>.Ok(nueva.CategoryID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener categoría: {ex.Message}");
        }
    }
}
