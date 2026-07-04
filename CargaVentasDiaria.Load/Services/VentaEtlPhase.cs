using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models.Csv;
using CargaVentasDiaria.Load.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace CargaVentasDiaria.Load.Services;

public class VentaEtlPhase : IEtlPhase
{
    private readonly CsvReaderService _csvReader;
    private readonly EtlMetrics _metrics;

    public VentaEtlPhase(CsvReaderService csvReader, EtlMetrics metrics)
    {
        _csvReader = csvReader;
        _metrics = metrics;
    }

    public async Task ExecuteAsync(IServiceProvider services, string rutaCsv)
    {
        Console.WriteLine("===== FASE C: Ventas =====");
        var resultados = _csvReader.Leer<VentaCsv>(rutaCsv);
        var validos = resultados.Where(r => r.Error == null).ToList();
        var errores = resultados.Where(r => r.Error != null).ToList();

        foreach (var (_, fila, error) in errores)
            _metrics.VentaRechazada(fila, error!, "Ventas");

        _metrics.VentasLeidas = resultados.Count;

        var grupos = validos
            .Select(r => new { Fila = r.Fila, Registro = r.Registro! })
            .GroupBy(x => x.Registro.NumeroOrden)
            .ToList();

        using var scope = services.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
        var orderStatusService = scope.ServiceProvider.GetRequiredService<IOrderStatusService>();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var orderDetailService = scope.ServiceProvider.GetRequiredService<IOrderDetailService>();

        foreach (var grupo in grupos)
        {
            var numeroOrden = grupo.Key;

            try
            {
                var primerRegistro = grupo.First().Registro;

                if (!DateTime.TryParse(primerRegistro.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                {
                    foreach (var item in grupo)
                        _metrics.VentaRechazada(item.Fila, $"Fecha inválida: {primerRegistro.Fecha}", "Ventas");
                    continue;
                }

                var custResult = await customerService.BuscarPorEmailAsync(primerRegistro.EmailCliente);
                if (!custResult.Success)
                {
                    foreach (var item in grupo)
                        _metrics.VentaRechazada(item.Fila, $"Cliente no encontrado: {primerRegistro.EmailCliente}", "Ventas");
                    continue;
                }

                var statusResult = await orderStatusService.ObtenerOCrearAsync(primerRegistro.Estado);
                if (!statusResult.Success)
                {
                    foreach (var item in grupo)
                        _metrics.VentaRechazada(item.Fila, $"Estado: {statusResult.Message}", "Ventas");
                    continue;
                }

                var orderResult = await orderService.ObtenerOCrearOrdenAsync(numeroOrden, custResult.Data, statusResult.Data, fecha);
                if (!orderResult.Success)
                {
                    foreach (var item in grupo)
                        _metrics.VentaRechazada(item.Fila, $"Orden: {orderResult.Message}", "Ventas");
                    continue;
                }

                _metrics.OrdenesCreadas++;

                foreach (var item in grupo)
                {
                    var reg = item.Registro;

                    if (string.IsNullOrWhiteSpace(reg.NombreProducto))
                    {
                        _metrics.VentaRechazada(item.Fila, "NombreProducto vacío", "Ventas");
                        continue;
                    }

                    if (!int.TryParse(reg.Cantidad, NumberStyles.Any, CultureInfo.InvariantCulture, out var cantidad))
                    {
                        _metrics.VentaRechazada(item.Fila, $"Cantidad no es un número entero válido: {reg.Cantidad}", "Ventas");
                        continue;
                    }
                    if (!decimal.TryParse(reg.PrecioUnitario, NumberStyles.Any, CultureInfo.InvariantCulture, out var precioUnitario))
                    {
                        _metrics.VentaRechazada(item.Fila, $"PrecioUnitario no es un número válido: {reg.PrecioUnitario}", "Ventas");
                        continue;
                    }

                    if (cantidad <= 0)
                    {
                        _metrics.VentaRechazada(item.Fila, $"Cantidad debe ser > 0: {cantidad}", "Ventas");
                        continue;
                    }
                    if (precioUnitario < 0)
                    {
                        _metrics.VentaRechazada(item.Fila, $"PrecioUnitario inválido: {precioUnitario}", "Ventas");
                        continue;
                    }

                    var prodResult = await productService.BuscarPorNombreAsync(reg.NombreProducto);
                    if (!prodResult.Success)
                    {
                        _metrics.VentaRechazada(item.Fila, $"Producto no encontrado: {reg.NombreProducto}", "Ventas");
                        continue;
                    }

                    var existeDuplicado = await orderDetailService.ExisteDuplicadoAsync(orderResult.Data, prodResult.Data);
                    if (existeDuplicado)
                    {
                        _metrics.VentaRechazada(item.Fila, $"Detalle duplicado (OrderID={orderResult.Data}, ProductID={prodResult.Data})", "Ventas");
                        continue;
                    }

                    var detResult = await orderDetailService.InsertarDetalleAsync(orderResult.Data, prodResult.Data, cantidad, precioUnitario);
                    if (!detResult.Success)
                    {
                        _metrics.VentaRechazada(item.Fila, $"Detalle: {detResult.Message}", "Ventas");
                        continue;
                    }

                    _metrics.DetallesInsertados++;
                }
            }
            catch (Exception ex)
            {
                foreach (var item in grupo)
                    _metrics.VentaRechazada(item.Fila, $"Excepción en grupo: {ex.Message}", "Ventas");
            }
        }
    }
}
