using CargaVentasDiaria.Data;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IProductoService
{
    Task<Result<int>> ObtenerOCrearAsync(string codigo, string nombre, int categoriaId);
}
