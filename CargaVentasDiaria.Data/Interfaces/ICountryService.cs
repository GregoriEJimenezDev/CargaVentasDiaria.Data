using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface ICountryService
{
    Task<OperationResult<int>> ObtenerOCrearAsync(string nombre);
}
