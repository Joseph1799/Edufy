using Edufy_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Edufy_API.Data_Access
{
    public class CursoDAO
    {

        public static IEnumerable<Curso> ObtenerCursos()
        {
            string query = "SELECT * FROM Curso";

            using (SqlConnection connection = new SqlConnection(Connection.connectionRoute))
            {
                SqlCommand command = new SqlCommand(query, connection);
                List<Curso> cursos = new List<Curso>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Curso curso = new Curso
                        {
                            IdCurso = (int)reader["IdCurso"],
                            NombreCurso = (string)reader["NombreCurso"],
                            CodigoCurso = (string)reader["CodigoCurso"],
                            Descripcion = reader["Descripcion"] as string
                        };

                        cursos.Add(curso);
                    }

                    return cursos;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }
            }
        }

        public static IEnumerable<Curso> ObtenerCursosDeEstudiante(int idEstudiante)
        {
            string connectionString = Connection.connectionRoute; 
            string query = @"
                SELECT c.*
                FROM Curso c
                INNER JOIN InscripcionesCurso ic ON c.IdCurso = ic.IdCurso
                WHERE ic.IdEstudiante = @IdEstudiante";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                List<Curso> cursos = new List<Curso>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Curso curso = new Curso
                        {
                            IdCurso = (int)reader["IdCurso"],
                            NombreCurso = (string)reader["NombreCurso"],
                            CodigoCurso = (string)reader["CodigoCurso"],
                            Descripcion = reader["Descripcion"] as string
                        };

                        cursos.Add(curso);
                    }

                    return cursos;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }
            }
        }

        public static CursoDetallado CursoDeEstudianteDetalle(int idEstudiante, int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT c.NombreCurso AS NombreCurso,
                c.CodigoCurso AS CodigoCurso,
                c.Descripcion AS DescripcionCurso,
                p.Nombre AS NombreProfesor,
                p.Apellido AS ApellidoProfesor,
                p.CorreoElectronico AS CorreoElectronicoProfesor
                FROM Curso c
                INNER JOIN InscripcionesCurso ic ON c.IdCurso = ic.IdCurso
                INNER JOIN ProfesorCurso pc ON c.IdCurso = pc.IdCurso
                INNER JOIN Profesor p ON pc.IdProfesor = p.ID
                WHERE ic.IdEstudiante = @idEstudiante
                AND c.IdCurso = @idCurso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@IdCurso", idCurso);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    CursoDetallado cursoDetallado = new CursoDetallado();
                    if (reader.Read())
                    {
                        cursoDetallado.NombreCurso = reader["NombreCurso"].ToString();
                        cursoDetallado.DescripcionCurso = reader["DescripcionCurso"].ToString();
                        cursoDetallado.NombreProfesor = reader["NombreProfesor"].ToString();
                        cursoDetallado.ApellidoProfesor = reader["ApellidoProfesor"].ToString();
                        cursoDetallado.CorreoElectronicoProfesor = reader["CorreoElectronicoProfesor"].ToString();
                    }

                    return cursoDetallado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }

            }
        }

        public static CursoDetallado CursoDetalle(int idProfesor, int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT c.NombreCurso AS NombreCurso,
                c.CodigoCurso AS CodigoCurso,
                c.Descripcion AS DescripcionCurso,
                p.Nombre AS NombreProfesor,
                p.Apellido AS ApellidoProfesor,
                p.CorreoElectronico AS CorreoElectronicoProfesor
                FROM Curso c
                INNER JOIN ProfesorCurso pc ON c.IdCurso = pc.IdCurso
                INNER JOIN Profesor p ON pc.IdProfesor = p.ID
                WHERE pc.IdProfesor = @idProfesor
                AND c.IdCurso = @idCurso;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProfesor", idProfesor);
                command.Parameters.AddWithValue("@IdCurso", idCurso);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    CursoDetallado cursoDetallado = new CursoDetallado();
                    if (reader.Read())
                    {
                        cursoDetallado.NombreCurso = reader["NombreCurso"].ToString();
                        cursoDetallado.DescripcionCurso = reader["DescripcionCurso"].ToString();
                        cursoDetallado.NombreProfesor = reader["NombreProfesor"].ToString();
                        cursoDetallado.ApellidoProfesor = reader["ApellidoProfesor"].ToString();
                        cursoDetallado.CorreoElectronicoProfesor = reader["CorreoElectronicoProfesor"].ToString();
                    }

                    return cursoDetallado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }

            }
        }

        public static IEnumerable<Curso> ObtenerCursosDeProfesor(int idProfesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT c.*
                FROM Curso c
                INNER JOIN ProfesorCurso ic ON c.IdCurso = ic.IdCurso
                WHERE ic.IdProfesor = @idProfesor";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idProfesor", idProfesor);
                List<Curso> cursos = new List<Curso>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Curso curso = new Curso
                        {
                            IdCurso = (int)reader["IdCurso"],
                            NombreCurso = (string)reader["NombreCurso"],
                            CodigoCurso = (string)reader["CodigoCurso"],
                            Descripcion = reader["Descripcion"] as string
                        };

                        cursos.Add(curso);
                    }

                    return cursos;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }
            }
        }

        public static IEnumerable<Estudiante> ObtenerEstudiantesDeCurso(int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT e.Nombre, e.Apellido, e.CorreoElectronico, e.Id
                FROM InscripcionesCurso ic
                JOIN Estudiante e ON ic.IdEstudiante = e.ID
                WHERE ic.IdCurso = @idCurso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idCurso", idCurso);
                List<Estudiante> estudiantes = new List<Estudiante>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Estudiante estudiante = new Estudiante
                        {
                            Id = (int)reader["Id"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            CorreoElectronico = (string)reader["CorreoElectronico"]
                        };

                        estudiantes.Add(estudiante);
                    }
                    return estudiantes;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los estudiantes de la base de datos.", ex);
                }
            }
        }

        public static void InscribirEstudianteACurso(int idEstudiante, int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                        IF NOT EXISTS (
                            SELECT 1
                            FROM InscripcionesCurso
                            WHERE IdEstudiante = @idEstudiante
                            AND IdCurso = @idCurso
                        )
                        BEGIN
                            INSERT INTO InscripcionesCurso (IdEstudiante, IdCurso, FechaInscripcion)
                            VALUES (@idEstudiante, @idCurso, @fechaInscripcion)
                        END";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@idCurso", idCurso);
                command.Parameters.AddWithValue("@fechaInscripcion", DateTime.Now);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                } catch (Exception ex)
                {
                    throw new Exception("Error al inscribir al estudiante en el curso.", ex);
                }
            }
        }

        public static void RemoverEstudianteDeCurso(int idEstudiante, int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                DELETE FROM InscripcionesCurso
                WHERE IdEstudiante = @idEstudiante
                AND IdCurso = @idCurso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@idCurso", idCurso);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al remover al estudiante del curso.", ex);
                }
            }
        }

        public class CursoDetallado
        {
            public string NombreCurso { get; set; }
            public string DescripcionCurso { get; set; }
            public string NombreProfesor { get; set; }
            public string ApellidoProfesor { get; set; }
            public string CorreoElectronicoProfesor { get; set; }
        }

    }
}