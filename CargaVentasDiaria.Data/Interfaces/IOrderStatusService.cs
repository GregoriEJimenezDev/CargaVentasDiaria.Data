using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IOrderStatusService
{
    Task<OperationResult<int>> ObtenerOCrearAsync(string nombre);
}
