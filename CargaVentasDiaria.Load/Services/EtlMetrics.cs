namespace CargaVentasDiaria.Load.Services;

public class EtlMetrics
{
    public int ProductosLeidos { get; private set; }
    public int ProductosInsertados { get; private set; }
    public List<(int Fila, string Motivo)> ProductosRechazados { get; } = new();

    public int ClientesLeidos { get; private set; }
    public int ClientesInsertados { get; private set; }
    public List<(int Fila, string Motivo)> ClientesRechazados { get; } = new();

    public int VentasLeidas { get; set; }
    public int OrdenesCreadas { get; set; }
    public int DetallesInsertados { get; set; }
    public List<(int Fila, string Motivo, string Archivo)> VentasRechazados { get; } = new();

    public void ProductoLeido() => ProductosLeidos++;
    public void ProductoInsertado() => ProductosInsertados++;
    public void ProductoRechazado(int fila, string motivo) => ProductosRechazados.Add((fila, motivo));

    public void ClienteLeido() => ClientesLeidos++;
    public void ClienteInsertado() => ClientesInsertados++;
    public void ClienteRechazado(int fila, string motivo) => ClientesRechazados.Add((fila, motivo));

    public void VentaRechazada(int fila, string motivo, string archivo) => VentasRechazados.Add((fila, motivo, archivo));

    public void ImprimirResumen()
    {
        Console.WriteLine();
        Console.WriteLine("===== RESUMEN DEL PROCESO ETL =====");

        Console.WriteLine("--- Productos ---");
        Console.WriteLine($"Leídos: {ProductosLeidos} | Insertados: {ProductosInsertados} | Rechazados: {ProductosRechazados.Count}");

        Console.WriteLine("--- Clientes ---");
        Console.WriteLine($"Leídos: {ClientesLeidos} | Insertados: {ClientesInsertados} | Rechazados: {ClientesRechazados.Count}");

        Console.WriteLine("--- Ventas (líneas de detalle) ---");
        Console.WriteLine($"Leídas: {VentasLeidas} | Órdenes creadas: {OrdenesCreadas} | Líneas insertadas: {DetallesInsertados} | Rechazadas: {VentasRechazados.Count}");

        var todosRechazos = new List<(int Fila, string Motivo, string Archivo)>();
        todosRechazos.AddRange(ProductosRechazados.Select(r => (r.Fila, r.Motivo, "Productos")));
        todosRechazos.AddRange(ClientesRechazados.Select(r => (r.Fila, r.Motivo, "Clientes")));
        todosRechazos.AddRange(VentasRechazados);

        if (todosRechazos.Count > 0)
        {
            Console.WriteLine("Detalle de rechazos (primeros 20):");
            foreach (var r in todosRechazos.Take(20))
                Console.WriteLine($"  [{r.Archivo}] Fila {r.Fila}: {r.Motivo}");
        }
    }
}
