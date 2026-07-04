using CargaVentasDiaria.Data.Result;

namespace CargaVentasDiaria.Data.Interfaces;

public interface ICustomerService
{
    Task<OperationResult<int>> ObtenerOCrearAsync(string firstName, string lastName, string email, string? phone, int cityId);
    Task<OperationResult<int>> BuscarPorEmailAsync(string email);
}
