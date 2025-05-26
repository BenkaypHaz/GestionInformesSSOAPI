public interface ICalculoEstrésTérmicoService
{
    string EvaluarRiesgo();
}

public class CalculoEstrésTérmicoService : ICalculoEstrésTérmicoService
{
    private readonly ITrabajadorRepository _repository;

    public CalculoEstrésTérmicoService(ITrabajadorRepository repository)
    {
        _repository = repository;
    }

    public string EvaluarRiesgo()
    {
        var datos = _repository.GetDatosTrabajador();
        double wbgt = CalculadorDeEstrésTérmico.CalcularWBGT(datos);
        string mensaje = CalculadorDeEstrésTérmico.CalcularBalanceDeCalor(datos);
        return $"WBGT: {wbgt}, {mensaje}";
    }

}
