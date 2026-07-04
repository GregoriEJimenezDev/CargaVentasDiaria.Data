namespace CargaVentasDiaria.Data.Models;

public class Country
{
    public int CountryID { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public ICollection<City> Cities { get; set; } = new List<City>();
}
