using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("PanelDatos_Calor")]
    public class PanelDatos_Calor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_panel")]
        public int IdPanel { get; set; }

        [Column("WBGT")]
        public decimal WBGT { get; set; }

        [Column("Bulbo_Seco")]
        public decimal BulboSeco { get; set; }

        [Column("Bulbo_humedo")]
        public decimal BulboHumedo { get; set; }

        [Column("Cuerpo_negro")]
        public decimal CuerpoNegro { get; set; }

        [Column("Indice_termico")]
        public decimal IndiceTermico { get; set; }

        [Column("Humedad_promedio")]
        public decimal HumedadPromedio { get; set; }
        [Column("id_informe")]
        public int Id_informe { get; set; }
    }
}
