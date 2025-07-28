// GestionsInformesSSOAPI/Infraestructure/Entities/CriteriosEvaluacion.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("CriteriosEvaluacion")]
    public class CriteriosEvaluacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_criterio")]
        public int IdCriterio { get; set; }

        [Column("id_informe")]
        public int IdInforme { get; set; }

        [Column("tiempo_exposicion_horas")]
        public decimal TiempoExposicionHoras { get; set; }

        [Column("peso_promedio_trabajadores")]
        public decimal PesoPromedioTrabajadores { get; set; }

        [Column("tasa_metabolica_valor")]
        public decimal TasaMetabolicaValor { get; set; }

        [Column("tasa_metabolica_descripcion")]
        public string TasaMetabolicaDescripcion { get; set; }
        [Column("ajuste_ropa_id")]
        public int? AjusteRopaId { get; set; }
        [Column("adaptacion_fisiologica_texto")]
        public string AdaptacionFisiologicaTexto { get; set; }

        [Column("ajuste_ropa_valor_clo")]
        public decimal? AjusteRopaValorClo { get; set; }
    }
}