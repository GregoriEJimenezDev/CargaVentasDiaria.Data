using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IOrderDetailService
{
    Task<OperationResult> InsertarDetalleAsync(int orderId, int productId, int quantity, decimal unitPrice);
    Task<bool> ExisteDuplicadoAsync(int orderId, int productId);
}
