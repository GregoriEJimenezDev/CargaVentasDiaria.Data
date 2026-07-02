namespace CargaVentasDiaria.Data.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
