using System.ComponentModel.DataAnnotations.Schema;

namespace CargaVentasDiaria.Data.Models;

public class OrderDetail
{
    public int DetailID { get; set; }
    public int OrderID { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductID { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal TotalPrice { get; set; }
}
