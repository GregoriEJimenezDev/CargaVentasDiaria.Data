using CargaVentasDiaria.Data;

namespace CargaVentasDiaria.Data.Interfaces;

public interface ICategoriaService
{
    Task<Result<int>> ObtenerOCrearAsync(string nombre);
}
