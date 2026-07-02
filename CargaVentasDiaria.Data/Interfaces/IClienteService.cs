using CargaVentasDiaria.Data;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IClienteService
{
    Task<Result<int>> ObtenerOCrearAsync(string codigo, string nombre);
}
