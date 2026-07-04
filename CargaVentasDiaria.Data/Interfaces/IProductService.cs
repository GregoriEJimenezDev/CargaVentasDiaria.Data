using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IProductService
{
    Task<OperationResult<int>> ObtenerOCrearAsync(string nombreProducto, int categoryId, decimal price, int stock);
    Task<OperationResult<int>> BuscarPorNombreAsync(string nombreProducto);
}
