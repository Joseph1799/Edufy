using Edufy_API.Data_Access;
using Edufy_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            var profesor = ProfesorDAO.Login(loginRequest.CorreoElectronico, loginRequest.Contrasenia, loginRequest.Rol);

            if (profesor == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("tu_clave_secreta_para_generar_el_token");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, profesor.Id.ToString()),
            new Claim("CorreoElectronico", profesor.CorreoElectronico),
            new Claim("Rol", loginRequest.Rol)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
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

        // GET api/profesor/datos
        [HttpGet]
        [Route("api/profesor/datos")]
        public IHttpActionResult GetProfesor()
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
                var profesor = ProfesorDAO.GetProfesorById(id);

                if (profesor == null)
                {
                    return NotFound();
                }

                return Ok(profesor);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // PUT api/profesor/modificar
        [HttpPut]
        [Route("api/profesor/modificar")]
        public IHttpActionResult ModificarProfesor([FromBody] Profesor profesor)
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
                profesor.Id = id; // Asignar el Id del profesor desde el token al objeto profesor

                if (!ProfesorDAO.ModificarProfesor(profesor))
                {
                    return InternalServerError(new Exception("No se pudo modificar al profesor"));
                }

                return Ok("Profesor modificado exitosamente");
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // PUT api/profesor/modificar/contrasenia
        [HttpPut]
        [Route("api/profesor/modificar/contrasenia")]
        public IHttpActionResult ModificarContrasenia([FromBody] Profesor profesor)
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
                profesor.Id = id; // Asignar el Id del profesor desde el token al objeto profesor

                if (!ProfesorDAO.ModificarContrasenia(profesor))
                {
                    return InternalServerError(new Exception("No se pudo modificar al profesor"));
                }

                return Ok("Profesor modificado exitosamente");
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }



    }
}