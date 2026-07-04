using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface ICategoryService
{
    Task<OperationResult<int>> ObtenerOCrearAsync(string nombre);
}
