using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("Empresas")]
    public class Empresas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_emp")]
        public int IdEmpresa { get; set; }

        [Column("nombre")]
        [StringLength(500)]
        public string? Nombre { get; set; }
        [Column("direccion")]
        [StringLength(1000)]
        public string? Direccion { get; set; }

    }
}
