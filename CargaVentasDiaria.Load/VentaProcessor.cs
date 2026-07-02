using CargaVentasDiaria.Data.Interfaces;
using CargaVentasDiaria.Data.Models;
using CargaVentasDiaria.Data.Models.Csv;

namespace CargaVentasDiaria.Load;

public class VentaProcessor
{
    private readonly ICategoriaService _categoriaService;
    private readonly IProductoService _productoService;
    private readonly IClienteService _clienteService;
    private readonly IVentaService _ventaService;

    public VentaProcessor(
        ICategoriaService categoriaService,
        IProductoService productoService,
        IClienteService clienteService,
        IVentaService ventaService)
    {
        _categoriaService = categoriaService;
        _productoService = productoService;
        _clienteService = clienteService;
        _ventaService = ventaService;
    }

    public async Task<(int Insertados, List<(int Fila, string Motivo)> Rechazados)> ProcesarAsync(List<VentaCsv> registros)
    {
        var insertados = 0;
        var rechazados = new List<(int Fila, string Motivo)>();

        for (int i = 0; i < registros.Count; i++)
        {
            var resultado = await ProcesarUnoAsync(registros[i], i + 2);
            if (resultado.Insertado)
                insertados++;
            else
                rechazados.Add((i + 2, resultado.Motivo!));
        }

        return (insertados, rechazados);
    }

    private async Task<(bool Insertado, string? Motivo)> ProcesarUnoAsync(VentaCsv r, int fila)
    {
        try
        {
            var cat = await _categoriaService.ObtenerOCrearAsync(r.Categoria);
            if (!cat.Success) return (false, $"Categoría: {cat.Message}");

            var prod = await _productoService.ObtenerOCrearAsync(r.CodigoProducto, r.NombreProducto, cat.Data!);
            if (!prod.Success) return (false, $"Producto: {prod.Message}");

            var cli = await _clienteService.ObtenerOCrearAsync(r.CodigoCliente, r.NombreCliente);
            if (!cli.Success) return (false, $"Cliente: {cli.Message}");

            if (await _ventaService.ExisteAsync(r.Fecha, cli.Data!, prod.Data!))
                return (false, "Venta duplicada.");

            var venta = new Venta
            {
                Fecha = r.Fecha.Date,
                ClienteId = cli.Data!,
                ProductoId = prod.Data!,
                Cantidad = r.Cantidad,
                PrecioUnitario = r.PrecioUnitario
            };

            var res = await _ventaService.InsertarAsync(venta);
            if (!res.Success) return (false, $"Venta: {res.Message}");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Excepción: {ex.Message}");
        }
    }
}
