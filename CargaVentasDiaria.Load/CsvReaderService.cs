using CargaVentasDiaria.Data.Models.Csv;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CargaVentasDiaria.Load;

public class CsvReaderService
{
    private readonly ValidadorVentaCsv _validador;

    public CsvReaderService(ValidadorVentaCsv validador)
    {
        _validador = validador;
    }

    public (List<VentaCsv> Validos, List<(int Fila, string Error)> Rechazados) Leer(string rutaCsv)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = null
        };

        var validos = new List<VentaCsv>();
        var rechazados = new List<(int Fila, string Error)>();

        using var reader = new StreamReader(rutaCsv);
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<VentaCsv>().ToList();

        for (int i = 0; i < records.Count; i++)
        {
            var error = _validador.Validar(records[i]);
            if (error == null)
                validos.Add(records[i]);
            else
                rechazados.Add((i + 2, error));
        }

        return (validos, rechazados);
    }
}
