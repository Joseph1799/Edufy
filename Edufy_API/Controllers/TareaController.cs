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
using System.Threading;
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

        [HttpGet]
        [Route("api/tarea/obtenerTareaPorId")]
        public IHttpActionResult ObtenerTareaPorId(int idTarea)
        {
            try
            {
                Tarea tarea = TareaDAO.ObtenerTareaPorId(idTarea);
                return Ok(tarea);
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

        // GET: api/Curso/estudiante/cursos
        [HttpGet]
        [Route("api/curso/estudiante/tareas-de-curso")]
        public IHttpActionResult TareasDeCursoDeEstudiante(int idCurso)
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
                    var tareas = TareaDAO.TareasDeCursoDeEstudiante(idEstudiante,idCurso);
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

        // GET: api/Curso/estudiante/cursos
        [HttpGet]
        [Route("api/curso/tareascurso")]
        public IHttpActionResult TareasDeCurso(int idCurso)
        {
                try
                {
                    var tareas = TareaDAO.TareasDeCurso(idCurso);
                    return Ok(tareas);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las tareas del estudiante.", ex);
                }
        }

        [HttpPost]
        [Route("api/curso/creartarea/{idCurso}")]
        public IHttpActionResult CrearTarea(int idCurso, [FromBody] Tarea tarea)
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
                    var idProfesor = int.Parse(idClaim.Value);
                    TareaDAO.CrearTarea(tarea, idCurso, idProfesor);
                    return Ok();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos del estudiante.", ex);
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // Entregar tarea de un estudiante en específico 
        [HttpPut]
        [Route("api/tarea/entregar-tarea/{docuEntregadoRuta}/{idTarea}")]
        public IHttpActionResult EntregarTarea(int idTarea, String docuEntregadoRuta)
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
                    TareaDAO.EntregarTarea(idTarea, idEstudiante, docuEntregadoRuta);
                    return Ok();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos del estudiante.", ex);
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // Revisar tarea de un estudiante en específico en un curso en específico 
        [HttpPut]
        [Route("api/tarea/revisar-tarea/{idEstudiante}/{puntaje}/{idCurso}")]
        public IHttpActionResult RevisarTarea(int idEstudiante, int puntaje, int idCurso)
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
                    var idProfesor = int.Parse(idClaim.Value);
                    TareaDAO.RevisarTarea(idEstudiante, puntaje, idCurso, idProfesor);
                    return Ok();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos del estudiante.", ex);
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // Obtiene todas las tareas creadas por un profesor en específico sin importar el curso
        [HttpGet]
        [Route("api/curso/profesor/tareas-creadas-profesor")]
        public IHttpActionResult TareasCreadasProfesor()
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
                    var idProfesor = int.Parse(idClaim.Value);
                    var tareas = TareaDAO.TareasCreadasProfesor(idProfesor);
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

        // Obtiene las tareas entregadas de un estudiante en específico
        [HttpGet]
        [Route("api/curso/estudiante/tareasEntregadas")]
        public IHttpActionResult TareasDeEstudianteEntregadas()
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
                    var tareas = TareaDAO.TareasDeEstudianteEntregadas(idEstudiante);
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

        // Obtiene las tareas entregadas de un curso en específico pertenente a un profesor en específico
        [HttpGet]
        [Route("api/curso/profesor/tareas-de-curso-entregadas")]
        public IHttpActionResult TareasDeCursoEntregadas(int idCurso) 
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
                    var idProfesor = int.Parse(idClaim.Value);
                    var tareas = TareaDAO.TareasDeCursoEntregadas(idProfesor, idCurso);
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


        // 
        [HttpGet]
        [Route("api/curso/estudiante/tarea-entregada-de-curso")]
        public IHttpActionResult TareaEntregadaDeCursoDeEstudiante(int idTarea, int idEstudiante, int idCurso)
        {
            try
            {
                var tareaEstudiante = TareaDAO.TareaEntregadaDeCursoDeEstudiante(idTarea, idEstudiante, idCurso);
                return Ok(tareaEstudiante);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las tareas del estudiante.", ex);
            }
         }

        [HttpDelete]
        [Route("api/curso/eliminar-tarea/{idTarea}")]
        public IHttpActionResult EliminarTarea(int idTarea)
        {
            try
            {
                TareaDAO.EliminarTarea(idTarea);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la tarea", ex);
            }
        }

        [HttpGet]
        [Route("api/curso/estudiante/tarea-revisada-de-curso")]
        public IHttpActionResult TareaRevisadaDeCursoDeEstudiante(int idCurso)
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
                    var tareaEstudiante = TareaDAO.TareaRevisadaDeCursoDeEstudiante(idEstudiante, idCurso);
                    return Ok(tareaEstudiante);
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

