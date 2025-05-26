using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    public class GraficoDatos_Calor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_registro")]
        public int IdRegistro { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("DryBulb")]
        public decimal DryBulb { get; set; }

        [Column("Humidity")]
        public decimal Humidity { get; set; }
        [Column("id_informe")]
        public int Id_informe { get; set; }
        [Column("id_area")]
        public int id_area { get; set; }
    }
}
