using CargaVentasDiaria.Data.Models.Csv;

namespace CargaVentasDiaria.Load;

public class ValidadorVentaCsv
{
    public string? Validar(VentaCsv r)
    {
        if (r.Fecha == default) return "Fecha inválida o vacía.";
        if (string.IsNullOrWhiteSpace(r.CodigoCliente)) return "CodigoCliente vacío.";
        if (string.IsNullOrWhiteSpace(r.NombreCliente)) return "NombreCliente vacío.";
        if (string.IsNullOrWhiteSpace(r.CodigoProducto)) return "CodigoProducto vacío.";
        if (string.IsNullOrWhiteSpace(r.NombreProducto)) return "NombreProducto vacío.";
        if (string.IsNullOrWhiteSpace(r.Categoria)) return "Categoria vacía.";
        if (r.Cantidad <= 0) return "Cantidad debe ser mayor a 0.";
        if (r.PrecioUnitario <= 0) return "PrecioUnitario debe ser mayor a 0.";
        return null;
    }
}
