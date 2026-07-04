namespace CargaVentasDiaria.Data.Models.Csv;

public class ClienteCsv
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string Ciudad { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
}
