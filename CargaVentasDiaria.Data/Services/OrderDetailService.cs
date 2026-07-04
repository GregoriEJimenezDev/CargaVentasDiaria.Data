using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly DbVentasContext _context;

    public OrderDetailService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> InsertarDetalleAsync(int orderId, int productId, int quantity, decimal unitPrice)
    {
        try
        {
            var detalle = new OrderDetail
            {
                OrderID = orderId,
                ProductID = productId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
            _context.OrderDetails.Add(detalle);
            await _context.SaveChangesAsync();
            return OperationResult.Ok();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Error al insertar detalle de orden: {ex.Message}");
        }
    }

    public async Task<bool> ExisteDuplicadoAsync(int orderId, int productId)
    {
        return await _context.OrderDetails
            .AsNoTracking()
            .AnyAsync(d => d.OrderID == orderId && d.ProductID == productId);
    }
}
