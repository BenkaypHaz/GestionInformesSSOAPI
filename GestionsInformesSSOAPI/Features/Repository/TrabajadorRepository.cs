public interface ITrabajadorRepository
{
    DatosTrabajador GetDatosTrabajador();
}

public class TrabajadorRepository : ITrabajadorRepository
{
    public DatosTrabajador GetDatosTrabajador()
    {
        // Devuelve datos basados en la última tabla proporcionada.
        return new DatosTrabajador
        {
            M = 250, // Tasa metabólica en W/m²
            W = 0,   // Trabajo mecánico efectivo en W (no especificado, asumiendo cero)
            Tair = 40, // Temperatura del aire en °C
            Tg = 43, // Temperatura del globo en °C
            Twb = 37, // Temperatura de bulbo húmedo en °C
            Vair = 0.2, // Velocidad del aire en m/s
            Icl = 0.50, // Aislamiento térmico de la ropa en clo
            Emax = 0, // Capacidad máxima de evaporación (no proporcionado, requeriría cálculo adicional)
            Hr = 0.70, // Humedad relativa en fracción (70%)
            Tskin = 35 // Temperatura de la piel en °C
        };
    }

}
