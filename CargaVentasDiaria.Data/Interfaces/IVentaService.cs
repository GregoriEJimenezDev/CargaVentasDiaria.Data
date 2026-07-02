using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data;

namespace CargaVentasDiaria.Data.Interfaces;

public interface IVentaService
{
    Task<Result> InsertarAsync(Venta venta);
    Task<bool> ExisteAsync(DateTime fecha, int clienteId, int productoId);
}
