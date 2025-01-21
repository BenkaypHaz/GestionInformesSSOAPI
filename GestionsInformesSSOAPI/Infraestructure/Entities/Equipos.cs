using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("Equipos")]
    public class Equipos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_equipo")]
        public int IdEquipo { get; set; }

        [Column("nombre")]
        [StringLength(500)]
        public string Nombre { get; set; }

        [Column("modelo")]
        [StringLength(500)]
        public string Modelo { get; set; }

        [Column("marca")]
        [StringLength(500)]
        public string Marca { get; set; }

        [Column("serie")]
        [StringLength(500)]
        public string Serie { get; set; }
    }
}
