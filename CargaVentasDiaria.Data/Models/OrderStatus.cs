namespace CargaVentasDiaria.Data.Models;

public class OrderStatus
{
    public int StatusID { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
