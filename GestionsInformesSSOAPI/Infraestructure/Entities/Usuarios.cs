﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionsInformesSSOAPI.Infraestructure.Entities
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("usuario")]
        [StringLength(255)]
        public string? Usuario { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(255)]
        public string? Nombre { get; set; }


        [Required]
        [Column("clave_encriptada")]
        [StringLength(255)]
        public string? ClaveEncriptada { get; set; }

        [Required]
        [Column("correo")]
        [StringLength(255)]
        [EmailAddress]
        public string? Correo { get; set; }


        [Column("id_rol")]
        public int? IdRol { get; set; }

        [Column("activo")]
        public bool? activo { get; set; }

    }
}
