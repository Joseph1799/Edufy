using Edufy_API.Data_Access;
using Edufy_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace Edufy_API.Controllers
{
    public class CursoController : ApiController
    {

        // GET: api/curso/ObtenerCursos
        [HttpGet]
        [Route("api/curso/ObtenerCursos")]
        public IHttpActionResult ObtenerCursos()
        {
            try
            {
                IEnumerable<Curso> cursos = CursoDAO.ObtenerCursos();
                return Ok(cursos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Curso/estudiante/cursos
        [HttpGet]
        [Route("api/curso/estudiante/cursos")]
        public IHttpActionResult CursosDeEstudiante()
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
                    var cursos = CursoDAO.ObtenerCursosDeEstudiante(idEstudiante);
                    return Ok(cursos);
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

        // GET: api/Curso/estudiante/cursos
        [HttpGet]
        [Route("api/curso/estudiante/curso-detalle")]
        public IHttpActionResult CursoDeEstudianteDetalle(int idCurso)
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
                    var cursoDetallado = CursoDAO.CursoDeEstudianteDetalle(idEstudiante, idCurso);
                    return Ok(cursoDetallado);
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

        // GET: api/curso/cursodetalle
        [HttpGet]
        [Route("api/curso/cursodetalle")]
        public IHttpActionResult CursoDetalle(int idCurso)
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
                    var cursoDetallado = CursoDAO.CursoDetalle(idProfesor, idCurso);
                    return Ok(cursoDetallado);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los detalles del curso.", ex);
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized();
            }
        }

        // GET: api/Curso/profesor/cursos
        [HttpGet]
        [Route("api/curso/profesor/cursos")]
        public IHttpActionResult CursosDeProfesor()
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
                    var cursos = CursoDAO.ObtenerCursosDeProfesor(idProfesor);
                    return Ok(cursos);
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

        [HttpGet]
        [Route("api/curso/estudiantes-de-curso")]
        public IHttpActionResult EstudiantesDeCurso(int idCurso)
        {
            try
            {
                IEnumerable<Estudiante> estudiantes = CursoDAO.ObtenerEstudiantesDeCurso(idCurso);
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/curso/inscribir-estudiante")]
        public IHttpActionResult InscribirEstudianteACurso(int idEstudiante, int idCurso)
        {
            if( idEstudiante == 0 && idCurso == 0)
            {
                return BadRequest();
            } else
            {
                CursoDAO.InscribirEstudianteACurso(idEstudiante, idCurso);
                return Ok("Estudiante inscrito exitosamente");
            }
        }

        [HttpDelete]
        [Route("api/curso/remover-estudiante")]
        public IHttpActionResult RemoverEstudianteDeCurso(int idEstudiante, int idCurso)
        {
            if (idEstudiante == 0 && idCurso == 0)
            {
                return BadRequest();
            }
            else
            {
                CursoDAO.RemoverEstudianteDeCurso(idEstudiante, idCurso);
                return Ok("Estudiante removido del curso exitosamente");
            }
        }

        [HttpPut]
        [Route("api/curso/agregar-estudiante-a-grupo")]
        public IHttpActionResult AgregarEstudianteAGrupo(int idEstudiante, int idCurso, int numGrupo)
        {
            if (idEstudiante == 0 && idCurso == 0)
            {
                return BadRequest();
            }
            else
            {
                CursoDAO.AgregarEstudianteAGrupo(idEstudiante, idCurso, numGrupo);
                return Ok("Estudiante agregado exitosamente en el grupo");
            }
        }

        [HttpGet]
        [Route("api/curso/estudiantes-de-grupo")]
        public IHttpActionResult EstudiantesDeGrupo(int idCurso, int numGrupo)
        {
            try
            {
                IEnumerable<Estudiante> estudiantes = CursoDAO.OtenerEstudiantesPorGrupo(idCurso, numGrupo);
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }

}