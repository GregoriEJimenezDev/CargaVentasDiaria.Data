using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class CountryService : ICountryService
{
    private readonly DbVentasContext _context;

    public CountryService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearAsync(string nombre)
    {
        try
        {
            var nombreTrim = nombre.Trim().ToLowerInvariant();
            var pais = await _context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CountryName.Trim().ToLower() == nombreTrim);

            if (pais != null)
                return OperationResult<int>.Ok(pais.CountryID);

            var nuevo = new Country { CountryName = nombre.Trim() };
            _context.Countries.Add(nuevo);
            await _context.SaveChangesAsync();

            return OperationResult<int>.Ok(nuevo.CountryID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener país: {ex.Message}");
        }
    }
}
