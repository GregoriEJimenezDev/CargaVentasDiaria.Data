namespace CargaVentasDiaria.Load.Interfaces;

public interface IEtlPhase
{
    Task ExecuteAsync(IServiceProvider services, string rutaCsv);
}
