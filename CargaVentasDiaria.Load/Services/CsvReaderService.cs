using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace CargaVentasDiaria.Load.Services;

public class CsvReaderService
{
    public List<(T? Registro, int Fila, string? Error)> Leer<T>(string ruta)
    {
        if (!File.Exists(ruta))
            throw new InvalidOperationException($"No se encuentra el archivo CSV en: {ruta}");

        var configCsv = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = null
        };

        var resultados = new List<(T? Registro, int Fila, string? Error)>();
        using var reader = new StreamReader(ruta);
        using var csv = new CsvReader(reader, configCsv);
        csv.Read();
        csv.ReadHeader();

        var fila = 1;
        while (csv.Read())
        {
            fila++;
            try
            {
                var record = csv.GetRecord<T>();
                resultados.Add((record, fila, null));
            }
            catch (Exception ex)
            {
                resultados.Add((default, fila, $"Error de conversión: {ex.Message}"));
            }
        }
        return resultados;
    }
}
