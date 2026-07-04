namespace CargaVentasDiaria.Data.Models.Csv;

public class VentaCsv
{
    public string NumeroOrden { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public string Cantidad { get; set; } = string.Empty;
    public string PrecioUnitario { get; set; } = string.Empty;
}
