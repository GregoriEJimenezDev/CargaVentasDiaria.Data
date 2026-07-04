using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models.Csv;
using CargaVentasDiaria.Load.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace CargaVentasDiaria.Load.Services;

public class ClienteEtlPhase : IEtlPhase
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private readonly CsvReaderService _csvReader;
    private readonly EtlMetrics _metrics;

    public ClienteEtlPhase(CsvReaderService csvReader, EtlMetrics metrics)
    {
        _csvReader = csvReader;
        _metrics = metrics;
    }

    public async Task ExecuteAsync(IServiceProvider services, string rutaCsv)
    {
        Console.WriteLine("===== FASE B: Clientes =====");
        var resultados = _csvReader.Leer<ClienteCsv>(rutaCsv);
        var emailsEnArchivo = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        using var scope = services.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        var cityService = scope.ServiceProvider.GetRequiredService<ICityService>();
        var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();

        foreach (var (registro, fila, error) in resultados)
        {
            _metrics.ClienteLeido();
            if (error != null)
            {
                _metrics.ClienteRechazado(fila, error);
                continue;
            }

            if (string.IsNullOrWhiteSpace(registro!.Nombre))
            {
                _metrics.ClienteRechazado(fila, "Nombre vacío");
                continue;
            }
            if (string.IsNullOrWhiteSpace(registro.Apellido))
            {
                _metrics.ClienteRechazado(fila, "Apellido vacío");
                continue;
            }
            if (string.IsNullOrWhiteSpace(registro.Email) || !EmailRegex.IsMatch(registro.Email))
            {
                _metrics.ClienteRechazado(fila, $"Email inválido: {registro.Email}");
                continue;
            }
            if (string.IsNullOrWhiteSpace(registro.Ciudad))
            {
                _metrics.ClienteRechazado(fila, "Ciudad vacía");
                continue;
            }
            if (string.IsNullOrWhiteSpace(registro.Pais))
            {
                _metrics.ClienteRechazado(fila, "Pais vacío");
                continue;
            }

            if (!emailsEnArchivo.Add(registro.Email))
            {
                _metrics.ClienteRechazado(fila, $"Email duplicado en el mismo archivo: {registro.Email}");
                continue;
            }

            try
            {
                var countryResult = await countryService.ObtenerOCrearAsync(registro.Pais);
                if (!countryResult.Success)
                {
                    _metrics.ClienteRechazado(fila, $"País: {countryResult.Message}");
                    continue;
                }

                var cityResult = await cityService.ObtenerOCrearAsync(registro.Ciudad, countryResult.Data);
                if (!cityResult.Success)
                {
                    _metrics.ClienteRechazado(fila, $"Ciudad: {cityResult.Message}");
                    continue;
                }

                var custResult = await customerService.ObtenerOCrearAsync(registro.Nombre, registro.Apellido, registro.Email, registro.Telefono, cityResult.Data);
                if (!custResult.Success)
                {
                    _metrics.ClienteRechazado(fila, $"Cliente: {custResult.Message}");
                    continue;
                }

                _metrics.ClienteInsertado();
            }
            catch (Exception ex)
            {
                _metrics.ClienteRechazado(fila, $"Excepción: {ex.Message}");
            }
        }
    }
}
