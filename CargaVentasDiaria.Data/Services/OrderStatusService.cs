using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class OrderStatusService : IOrderStatusService
{
    private readonly DbVentasContext _context;

    public OrderStatusService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearAsync(string nombre)
    {
        try
        {
            var nombreTrim = nombre.Trim().ToLowerInvariant();
            var estado = await _context.OrderStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StatusName.Trim().ToLower() == nombreTrim);

            if (estado != null)
                return OperationResult<int>.Ok(estado.StatusID);

            var nuevo = new OrderStatus { StatusName = nombre.Trim() };
            _context.OrderStatuses.Add(nuevo);
            await _context.SaveChangesAsync();

            return OperationResult<int>.Ok(nuevo.StatusID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener estado de orden: {ex.Message}");
        }
    }
}
