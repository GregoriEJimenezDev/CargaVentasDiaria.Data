using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class CategoriaService : ICategoriaService
{
    private readonly DbVentasContext _context;

    public CategoriaService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> ObtenerOCrearAsync(string nombre)
    {
        try
        {
            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);

            if (categoria != null)
                return Result<int>.Ok(categoria.Id);

            var nueva = new Categoria { Nombre = nombre };
            _context.Categorias.Add(nueva);
            await _context.SaveChangesAsync();

            return Result<int>.Ok(nueva.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error al crear/obtener categoría: {ex.Message}");
        }
    }
}
