using CargaVentasDiaria.Data.Context;
using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Services;
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
            var rutaCsv = ObtenerRutaCsv(config);
            var provider = ConfigurarServicios(config);

            var lector = new CsvReaderService(new ValidadorVentaCsv());
            var (validos, erroresLectura) = lector.Leer(rutaCsv);

            using var scope = provider.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<VentaProcessor>();
            var (insertados, erroresProceso) = await processor.ProcesarAsync(validos);

            ImprimirResumen(validos.Count + erroresLectura.Count, insertados, erroresLectura, erroresProceso);

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
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

    private static string ObtenerRutaCsv(IConfiguration config)
    {
        var ruta = config["CsvSettings:RutaArchivo"]
            ?? throw new InvalidOperationException("CsvSettings:RutaArchivo no está configurado.");

        if (!File.Exists(ruta))
            throw new InvalidOperationException($"No se encuentra el archivo CSV en: {ruta}");

        return ruta;
    }

    private static ServiceProvider ConfigurarServicios(IConfiguration config)
    {
        var services = new ServiceCollection();
        var connectionString = config.GetConnectionString("VentasDb")
            ?? throw new InvalidOperationException("ConnectionStrings:VentasDb no está configurado.");

        services.AddDbContext<DbVentasContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IVentaService, VentaService>();
        services.AddScoped<VentaProcessor>();

        return services.BuildServiceProvider();
    }

    private static void ImprimirResumen(
        int totalLeidos, int insertados,
        List<(int Fila, string Error)> erroresLectura,
        List<(int Fila, string Motivo)> erroresProceso)
    {
        Console.WriteLine();
        Console.WriteLine("===== RESUMEN =====");
        Console.WriteLine($"Leídos:     {totalLeidos}");
        Console.WriteLine($"Insertados: {insertados}");
        Console.WriteLine($"Rechazados: {erroresLectura.Count + erroresProceso.Count}");

        if (erroresLectura.Count > 0)
        {
            Console.WriteLine("Errores de validación:");
            for (int i = 0; i < erroresLectura.Count; i++)
                Console.WriteLine($"  Fila {erroresLectura[i].Fila}: {erroresLectura[i].Error}");
        }

        if (erroresProceso.Count > 0)
        {
            Console.WriteLine("Errores de proceso:");
            for (int i = 0; i < erroresProceso.Count; i++)
                Console.WriteLine($"  Fila {erroresProceso[i].Fila}: {erroresProceso[i].Motivo}");
        }
    }
}
