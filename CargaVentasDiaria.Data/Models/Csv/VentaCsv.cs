namespace CargaVentasDiaria.Data.Models.Csv;

public class VentaCsv
{
    public DateTime Fecha { get; set; }
    public string CodigoCliente { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public string CodigoProducto { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
