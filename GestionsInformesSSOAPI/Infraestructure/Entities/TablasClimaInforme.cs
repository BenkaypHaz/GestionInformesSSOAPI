using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("TablasClima_Informe")]
    public class TablasClimaInforme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_tabla")]
        public int IdTabla { get; set; }

        [Column("id_informe")]
        public int IdInforme { get; set; }

        [Column("humedad_Relativa")]
        [Precision(6, 2)]
        public decimal HumedadRelativa { get; set; }

        [Column("temperatura_Maxima")]
        [Precision(6, 2)]
        public decimal TemperaturaMaxima { get; set; }

        [Column("temperatura_Minima")]
        [Precision(6, 2)]
        public decimal TemperaturaMinima { get; set; }
        [Column("Nubosidad")]
        [StringLength(50)]
        public string? Nubosidad { get; set; }
        [Column("Hora_Inicio")]
        [StringLength(10)]
        public string? HoraInicio { get; set; }

        [Column("Hora_Finalizacion")]
        [StringLength(10)]
        public string? HoraFinalizacion { get; set; }
    }
}
