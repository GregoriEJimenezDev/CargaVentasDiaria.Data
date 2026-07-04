namespace CargaVentasDiaria.Data.Models;

public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public Customer Customer { get; set; } = null!;
    public int StatusID { get; set; }
    public OrderStatus Status { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
