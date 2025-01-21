using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("EquipoUtilizado_Informe")]
    public class EquipoUtilizadoInforme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_uti")]
        public int IdUti { get; set; }

        [Column("id_informe")]
        public int IdInforme { get; set; }

        [Column("id_equipo")]
        public int IdEquipo { get; set; }
    }
}
