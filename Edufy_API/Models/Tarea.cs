using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Edufy_API.Models
{
    public class Tarea
    {
        [Key]
        public int IdTarea { get; set; }

        [Required]
        public String NombreTarea { get; set; }

        [Required]
        public String Descripcion { get; set; }

        [Required]
        public String FechaInicio { get; set; }

        [Required]
        public String FechaLimite { get; set; }

        public String DocumentoRuta { get; set; }

        [Required]
        public int IdCurso { get; set; }

        [Required]
        public int IdProfesor { get; set; }

        public string NombreCurso { get; set; }

    }
}