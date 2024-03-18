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
    public class TareaController : ApiController
    {

        // GET: api/tarea/obtenerTareas
        [HttpGet]
        [Route("api/tarea/obtenerTareas")]
        public IHttpActionResult ObtenerTareas()
        {
            try
            {
                IEnumerable<Tarea> tareas = TareaDAO.ObtenerTareas();
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Curso/estudiante/cursos
        [HttpGet]
        [Route("api/curso/estudiante/tareas")]
        public IHttpActionResult TareasDeEstudiante()
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
                try
                {
                    var idEstudiante = int.Parse(idClaim.Value);
                    var tareas = TareaDAO.ObtenerTareasDeEstudiante(idEstudiante);
                    return Ok(tareas);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las tareas del estudiante.", ex);
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

    }
}