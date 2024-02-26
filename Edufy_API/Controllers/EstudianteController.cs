using Edufy_API.Data_Access;
using Edufy_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Edufy_API.Controllers
{
    public class EstudianteController : ApiController
    {
        // POST api/estudiante/registrar
        [HttpPost]
        [Route("api/estudiante/registrar")]
        public IHttpActionResult RegistrarEstudiante([FromBody] Estudiante estudiante)
        {
            if (estudiante == null)
            {
                return BadRequest("Los datos del estudiante no pueden estar vacíos");
            }

            if (!EstudianteDAO.Registrar(estudiante))
            {
                return InternalServerError(new Exception("No se pudo registrar al estudiante"));
            }

            return Ok("Estudiante registrado exitosamente");
        }

        // POST api/estudiante/login
        [HttpPost]
        [Route("api/estudiante/login")]
        public IHttpActionResult LoginEstudiante([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Los datos de inicio de sesión no pueden estar vacíos");
            }

            if (!EstudianteDAO.Login(loginRequest.CorreoElectronico, loginRequest.Contrasenia, loginRequest.Rol))
            {
                return Unauthorized();
            }

            return Ok("Inicio de sesión exitoso");
        }

        public class LoginRequest
        {
            public string CorreoElectronico { get; set; }
            public string Contrasenia { get; set; }
            public string Rol { get; set; }
        }


    }
}