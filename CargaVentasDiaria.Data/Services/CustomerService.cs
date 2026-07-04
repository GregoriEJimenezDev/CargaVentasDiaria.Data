using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Result;
using Microsoft.EntityFrameworkCore;

namespace CargaVentasDiaria.Data.Services;

public class CustomerService : ICustomerService
{
    private readonly DbVentasContext _context;

    public CustomerService(DbVentasContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<int>> ObtenerOCrearAsync(string firstName, string lastName, string email, string? phone, int cityId)
    {
        try
        {
            var emailTrim = email.Trim().ToLowerInvariant();
            var cliente = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email.Trim().ToLower() == emailTrim);

            if (cliente != null)
                return OperationResult<int>.Ok(cliente.CustomerID);

            var nuevo = new Customer
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim(),
                Phone = phone?.Trim(),
                CityID = cityId
            };
            _context.Customers.Add(nuevo);
            await _context.SaveChangesAsync();

            return OperationResult<int>.Ok(nuevo.CustomerID);
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al crear/obtener cliente: {ex.Message}");
        }
    }

    public async Task<OperationResult<int>> BuscarPorEmailAsync(string email)
    {
        try
        {
            var emailTrim = email.Trim().ToLowerInvariant();
            var cliente = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email.Trim().ToLower() == emailTrim);

            if (cliente != null)
                return OperationResult<int>.Ok(cliente.CustomerID);

            return OperationResult<int>.Fail($"Cliente con email '{email}' no encontrado.");
        }
        catch (Exception ex)
        {
            return OperationResult<int>.Fail($"Error al buscar cliente por email: {ex.Message}");
        }
    }
}
