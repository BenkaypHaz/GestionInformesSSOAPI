using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Ropa_Utilizada")]
public class RopaUtilizada
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id_ropa")]
    public int IdRopa { get; set; }

    [Column("nombre")]
    [Required]
    public string Nombre { get; set; }

    [Column("descripcion")]
    [Required]
    public string Descripcion { get; set; }

    [Column("cy_f")]
    [Required]
    public decimal CyF { get; set; }
}
