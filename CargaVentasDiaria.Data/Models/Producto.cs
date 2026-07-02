namespace CargaVentasDiaria.Data.Models;

public class Producto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
