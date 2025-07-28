// GestionsInformesSSOAPI/Infraestructure/Modelos/CriteriosEvaluacionDto.cs
namespace GestionsInformesSSOAPI.Infraestructure.Modelos
{
    public class CriteriosEvaluacionDto
    {
        public int IdInforme { get; set; }
        public decimal TiempoExposicionHoras { get; set; }
        public decimal PesoPromedioTrabajadores { get; set; }
        public decimal TasaMetabolicaValor { get; set; }
        public string TasaMetabolicaDescripcion { get; set; }
        public int? AjusteRopaId { get; set; }
        public string AdaptacionFisiologicaTexto { get; set; } 
        public decimal? AjusteRopaValorClo { get; set; }
    }
}