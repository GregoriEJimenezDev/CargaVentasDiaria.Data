using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class OrderService : IOrderService
{
    private readonly DbVentasContext _context;
    private readonly Dictionary<string, int> _ordenesCache = new(StringComparer.OrdinalIgnoreCase);

    public OrderService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearOrdenAsync(string numeroOrdenExterno, int customerId, int statusId, DateTime fecha)
    {
        try
        {
            if (_ordenesCache.TryGetValue(numeroOrdenExterno, out var orderIdExistente))
                return OperationResult<int>.Ok(orderIdExistente);

            var orden = new Order
            {
                CustomerID = customerId,
                StatusID = statusId,
                OrderDate = fecha
            };
            _context.Orders.Add(orden);
            await _context.SaveChangesAsync();

            _ordenesCache[numeroOrdenExterno] = orden.OrderID;
            return OperationResult<int>.Ok(orden.OrderID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener orden: {ex.Message}");
        }
    }
}
