using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface ICityService
{
    Task<OperationResult<int>> ObtenerOCrearAsync(string nombre, int countryId);
}
