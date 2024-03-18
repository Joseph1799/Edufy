using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Edufy_API.Models
{
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }

        [Required]
        public string NombreCurso { get; set; }

        [Required]
        public string CodigoCurso { get; set; }


        public string Descripcion { get; set; }

    }
}