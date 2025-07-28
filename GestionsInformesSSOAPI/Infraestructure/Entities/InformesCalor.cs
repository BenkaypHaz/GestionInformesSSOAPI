using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("InformesCalor")]
    public class InformesCalor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_info")]
        public int IdInfo { get; set; }

        [Column("id_empresa")]
        public int IdEmpresa { get; set; }

        [Column("esAfiliada")]
        public bool EsAfiliada { get; set; }

        [Column("fechaInicia")]
        public DateTime FechaInicia { get; set; }

        [Column("fechaFinaliza")]
        public DateTime FechaFinaliza { get; set; }

        [Column("id_tecnico")]
        public int IdTecnico { get; set; }
        [Column("Peso_Promedio")]
        public decimal Peso_Promedio { get; set; }
        [Column("Aclimatacion")]
        public bool Aclimatacion { get; set; } // NUEVO CAMPO

        [Column("Hidratacion")]
        public bool Hidratacion { get; set; } // NUEVO 
        [Column("Conclusiones")]
        public string Conclusiones { get; set; }
    }
}
