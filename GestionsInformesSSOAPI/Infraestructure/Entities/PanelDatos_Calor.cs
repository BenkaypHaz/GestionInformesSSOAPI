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
        [Column("id_area")]
        public int id_area { get; set; }
        [Column("GenerarGraficoCampana")]
        public bool GenerarGraficoCampana { get; set; } // Nuevo campo
        public decimal PMV { get; set; } // Nuevo campo
        public decimal PPD { get; set; } // Nuevo campo

        [Column("Tasa_Estimada")]
        public decimal Tasa_Estimada { get; set; }
        [Column("IdRopaUtilizada")]
        public int? IdRopaUtilizada { get; set; }

        [Column("Postura")]
        public string Postura { get; set; } // NUEVO CAMPO

        [Column("Ambiente")]
        public string Ambiente { get; set; } // NUEVO CAMPO
    }
}
