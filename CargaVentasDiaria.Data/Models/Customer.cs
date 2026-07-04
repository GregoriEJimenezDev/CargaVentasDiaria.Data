namespace CargaVentasDiaria.Data.Models;

public class Customer
{
    public int CustomerID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int CityID { get; set; }
    public City City { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
