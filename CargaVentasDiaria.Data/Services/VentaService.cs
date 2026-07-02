using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class VentaService : IVentaService
{
    private readonly DbVentasContext _context;

    public VentaService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<Result> InsertarAsync(Venta venta)
    {
        try
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error al insertar venta: {ex.Message}");
        }
    }

    public async Task<bool> ExisteAsync(DateTime fecha, int clienteId, int productoId)
    {
        return await _context.Ventas
            .AsNoTracking()
            .AnyAsync(v => v.Fecha.Date == fecha.Date
                        && v.ClienteId == clienteId
                        && v.ProductoId == productoId);
    }
}
