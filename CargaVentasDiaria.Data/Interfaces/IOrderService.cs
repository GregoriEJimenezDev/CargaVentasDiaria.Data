using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IOrderService
{
    Task<OperationResult<int>> ObtenerOCrearOrdenAsync(string numeroOrdenExterno, int customerId, int statusId, DateTime fecha);
}
