using Edufy_API.Data_Access;
using Edufy_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
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

            var estudiante = EstudianteDAO.Login(loginRequest.CorreoElectronico, loginRequest.Contrasenia, loginRequest.Rol);

            if (estudiante == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("tu_clave_secreta_para_generar_el_token");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, estudiante.Id.ToString()),
            new Claim("CorreoElectronico", estudiante.CorreoElectronico),
            new Claim("Rol", loginRequest.Rol)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }


        public class LoginRequest
        {
            public string CorreoElectronico { get; set; }
            public string Contrasenia { get; set; }
            public string Rol { get; set; }
        }

        // GET api/estudiante/datos
        [HttpGet]
        [Route("api/estudiante/datos")]
        public IHttpActionResult GetEstudiante()
        {
            var authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader == null || authorizationHeader.Scheme != "Bearer")
            {
                return Unauthorized();
            }

            var token = authorizationHeader.Parameter;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("tu_clave_secreta_para_generar_el_token");
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var idClaim = principal.FindFirst(ClaimTypes.Name);
                if (idClaim == null)
                {
                    return Unauthorized();
                }

                var id = int.Parse(idClaim.Value);
                var estudiante = EstudianteDAO.GetEstudianteById(id);

                if (estudiante == null)
                {
                    return NotFound();
                }

                return Ok(estudiante);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // PUT api/estudiante/modificar
        [HttpPut]
        [Route("api/estudiante/modificar")]
        public IHttpActionResult ModificarEstudiante([FromBody] Estudiante estudiante)
        {
            var authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader == null || authorizationHeader.Scheme != "Bearer")
            {
                return Unauthorized();
            }

            var token = authorizationHeader.Parameter;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("tu_clave_secreta_para_generar_el_token");
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var idClaim = principal.FindFirst(ClaimTypes.Name);
                if (idClaim == null)
                {
                    return Unauthorized();
                }

                var id = int.Parse(idClaim.Value);
                estudiante.Id = id; // Asignar el Id del estudiante desde el token al objeto estudiante

                if (!EstudianteDAO.ModificarEstudiante(estudiante))
                {
                    return InternalServerError(new Exception("No se pudo modificar al estudiante"));
                }

                return Ok("Estudiante modificado exitosamente");
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // PUT api/estudiante/modificar/contrasenia
        [HttpPut]
        [Route("api/estudiante/modificar/contrasenia")]
        public IHttpActionResult ModificarContrasenia([FromBody] Estudiante estudiante)
        {
            var authorizationHeader = Request.Headers.Authorization;
            if (authorizationHeader == null || authorizationHeader.Scheme != "Bearer")
            {
                return Unauthorized();
            }

            var token = authorizationHeader.Parameter;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("tu_clave_secreta_para_generar_el_token");
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var idClaim = principal.FindFirst(ClaimTypes.Name);
                if (idClaim == null)
                {
                    return Unauthorized();
                }

                var id = int.Parse(idClaim.Value);
                estudiante.Id = id; // Asignar el Id del estudiante desde el token al objeto estudiante

                if (!EstudianteDAO.ModificarContrasenia(estudiante))
                {
                    return InternalServerError(new Exception("No se pudo modificar al estudiante"));
                }

                return Ok("Estudiante modificado exitosamente");
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }




    }
}