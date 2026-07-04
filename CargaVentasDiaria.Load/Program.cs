using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Services;
using CargaVentasDiaria.Load.Interfaces;
using CargaVentasDiaria.Load.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CargaVentasDiaria.Load;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            var config = CrearConfiguracion();
            var provider = ConfigurarServicios(config);
            var rutas = config.GetSection("RutasCsv");

            using (var scope = provider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<DbVentasContext>();
                await ctx.Database.EnsureCreatedAsync();
            }

            var csvReader = new CsvReaderService();
            var metrics = new EtlMetrics();

            await new ProductoEtlPhase(csvReader, metrics).ExecuteAsync(provider, rutas["Productos"]!);
            await new ClienteEtlPhase(csvReader, metrics).ExecuteAsync(provider, rutas["Clientes"]!);
            await new VentaEtlPhase(csvReader, metrics).ExecuteAsync(provider, rutas["Ventas"]!);

            metrics.ImprimirResumen();
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR FATAL: {ex.Message}");
            return 1;
        }
    }

    private static IConfiguration CrearConfiguracion()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    private static ServiceProvider ConfigurarServicios(IConfiguration config)
    {
        var services = new ServiceCollection();
        var connectionString = config.GetConnectionString("VentasDb")
            ?? throw new InvalidOperationException("ConnectionStrings:VentasDb no está configurado.");

        services.AddDbContext<DbVentasContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IOrderStatusService, OrderStatusService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();

        return services.BuildServiceProvider();
    }
}
