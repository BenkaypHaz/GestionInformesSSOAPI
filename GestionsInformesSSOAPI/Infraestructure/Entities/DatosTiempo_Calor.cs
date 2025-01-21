using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    public class DatosTiempo_Calor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_dato")]
        public int IdDato { get; set; }

        [Column("Tiempo")]
        public TimeSpan Tiempo { get; set; }

        [Column("MS")]
        public decimal MS { get; set; }
        [Column("id_informe")]
        public int Id_informe { get; set; }
        [Column("id_area")]
        public int id_area { get; set; }
    }
}
