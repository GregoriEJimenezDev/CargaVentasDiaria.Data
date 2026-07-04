using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class CityService : ICityService
{
    private readonly DbVentasContext _context;

    public CityService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearAsync(string nombre, int countryId)
    {
        try
        {
            var nombreTrim = nombre.Trim().ToLowerInvariant();
            var ciudad = await _context.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CityName.Trim().ToLower() == nombreTrim
                                       && c.CountryID == countryId);

            if (ciudad != null)
                return OperationResult<int>.Ok(ciudad.CityID);

            var nueva = new City { CityName = nombre.Trim(), CountryID = countryId };
            _context.Cities.Add(nueva);
            await _context.SaveChangesAsync();

            return OperationResult<int>.Ok(nueva.CityID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener ciudad: {ex.Message}");
        }
    }
}
