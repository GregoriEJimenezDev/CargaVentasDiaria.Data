namespace CargaVentasDiaria.Data.Models;

public class City
{
    public int CityID { get; set; }
    public string CityName { get; set; } = string.Empty;
    public int CountryID { get; set; }
    public Country Country { get; set; } = null!;
    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
