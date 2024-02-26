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
    public class ProfesorController : ApiController
    {
        // POST api/profesor/registrar
        [HttpPost]
        [Route("api/profesor/registrar")]
        public IHttpActionResult RegistrarProfesor([FromBody] Profesor profesor)
        {
            if (profesor == null)
            {
                return BadRequest("Los datos del profesor no pueden estar vacíos");
            }

            if (!ProfesorDAO.Registrar(profesor))
            {
                return InternalServerError(new Exception("No se pudo registrar al profesor"));
            }

            return Ok("Profesor registrado exitosamente");
        }

        // POST api/profesor/login
        [HttpPost]
        [Route("api/profesor/login")]
        public IHttpActionResult LoginProfesor([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Los datos de inicio de sesión no pueden estar vacíos");
            }

            if (!ProfesorDAO.Login(loginRequest.CorreoElectronico, loginRequest.Contrasenia, loginRequest.Rol))
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