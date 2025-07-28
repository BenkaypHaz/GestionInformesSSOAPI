
using GestionsInformesSSOAPI.Infraestructure.Entities;

public class InformeRequest
{
    public int IdEmpresa { get; set; }
    public bool EsAfiliada { get; set; }
    public DateTime FechaInicia { get; set; }
    public DateTime FechaFinaliza { get; set; }
    public int IdTecnico { get; set; }
    public decimal Tasa_Estimada { get; set; }
    public decimal Peso_Promedio { get; set; }
    public bool Aclimatacion { get; set; } // NUEVO
    public bool Hidratacion { get; set; } // NUEVO
    public string Conclusiones { get; set; } // NUEVO
    public List<EquipoUtilizadoRequest> EquiposUtilizados { get; set; }
    public List<DiaEvaluacionRequest> DiasEvaluacion { get; set; }
    public List<TitulosGraficos> TitulosGraficos { get; set; }
}

public class EquipoUtilizadoRequest
{
    public int IdEquipo { get; set; }
}

public class DiaEvaluacionRequest
{
    public string Dia { get; set; }
    public string HumedadRelativa { get; set; }
    public string Nubosidad { get; set; }
    public string TemperaturaMaxima { get; set; }
    public string TemperaturaMinima { get; set; }
}


