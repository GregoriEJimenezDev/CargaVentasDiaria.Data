using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class ClienteService : IClienteService
{
    private readonly DbVentasContext _context;

    public ClienteService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> ObtenerOCrearAsync(string codigo, string nombre)
    {
        try
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Codigo == codigo);

            if (cliente != null)
                return Result<int>.Ok(cliente.Id);

            var nuevo = new Cliente { Codigo = codigo, Nombre = nombre };
            _context.Clientes.Add(nuevo);
            await _context.SaveChangesAsync();

            return Result<int>.Ok(nuevo.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error al crear/obtener cliente: {ex.Message}");
        }
    }
}
