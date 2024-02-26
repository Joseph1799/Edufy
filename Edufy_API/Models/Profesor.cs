using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Edufy_API.Models
{
    public class Profesor
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string CorreoElectronico { get; set; }

        [Required]
        public string Contrasenia { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Genero { get; set; }

        [Required]
        public string Departamento { get; set; }

        [Required]
        public int TituloAcademico { get; set; }

        [Required]
        public bool EstadoCuenta { get; set; }

        public byte[] FotoPerfil { get; set; }

        [Required]
        public string Rol { get; set; }

    }
}