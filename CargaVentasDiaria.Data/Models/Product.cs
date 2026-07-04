namespace CargaVentasDiaria.Data.Models;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryID { get; set; }
    public Category Category { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
