using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models.Csv;
using CargaVentasDiaria.Load.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace CargaVentasDiaria.Load.Services;

public class ProductoEtlPhase : IEtlPhase
{
    private readonly CsvReaderService _csvReader;
    private readonly EtlMetrics _metrics;

    public ProductoEtlPhase(CsvReaderService csvReader, EtlMetrics metrics)
    {
        _csvReader = csvReader;
        _metrics = metrics;
    }

    public async Task ExecuteAsync(IServiceProvider services, string rutaCsv)
    {
        Console.WriteLine("===== FASE A: Productos =====");
        var resultados = _csvReader.Leer<ProductoCsv>(rutaCsv);

        using var scope = services.CreateScope();
        var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

        foreach (var (registro, fila, error) in resultados)
        {
            _metrics.ProductoLeido();
            if (error != null)
            {
                _metrics.ProductoRechazado(fila, error);
                continue;
            }

            if (string.IsNullOrWhiteSpace(registro!.NombreProducto))
            {
                _metrics.ProductoRechazado(fila, "NombreProducto vacío");
                continue;
            }
            if (string.IsNullOrWhiteSpace(registro.Categoria))
            {
                _metrics.ProductoRechazado(fila, "Categoria vacía");
                continue;
            }

            if (!decimal.TryParse(registro.Precio, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
            {
                _metrics.ProductoRechazado(fila, $"Precio no es un número válido: {registro.Precio}");
                continue;
            }
            if (!int.TryParse(registro.Stock, out var stock))
            {
                _metrics.ProductoRechazado(fila, $"Stock no es un número entero válido: {registro.Stock}");
                continue;
            }

            if (precio < 0)
            {
                _metrics.ProductoRechazado(fila, $"Precio inválido: {precio}");
                continue;
            }
            if (stock < 0)
            {
                _metrics.ProductoRechazado(fila, $"Stock inválido: {stock}");
                continue;
            }

            try
            {
                var catResult = await categoryService.ObtenerOCrearAsync(registro.Categoria);
                if (!catResult.Success)
                {
                    _metrics.ProductoRechazado(fila, $"Categoría: {catResult.Message}");
                    continue;
                }

                var prodResult = await productService.ObtenerOCrearAsync(registro.NombreProducto, catResult.Data, precio, stock);
                if (!prodResult.Success)
                {
                    _metrics.ProductoRechazado(fila, $"Producto: {prodResult.Message}");
                    continue;
                }

                _metrics.ProductoInsertado();
            }
            catch (Exception ex)
            {
                _metrics.ProductoRechazado(fila, $"Excepción: {ex.Message}");
            }
        }
    }
}
