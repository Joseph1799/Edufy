using Edufy_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace Edufy_API.Data_Access
{
    public class TareaDAO
    {

        public static IEnumerable<Tarea> ObtenerTareas()
        {
            string query = "SELECT * FROM Tarea";

            using (SqlConnection connection = new SqlConnection(Connection.connectionRoute))
            {
                SqlCommand command = new SqlCommand(query, connection);
                List<Tarea> cursos = new List<Tarea>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea
                        {
                            IdTarea = (int)reader["IdTarea"],
                            NombreTarea = (string)reader["NombreTarea"],
                            Descripcion = (string)reader["Descripcion"],
                            FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd"),
                            FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd"),
                            DocumentoRuta = (string)reader["DocumentoRuta"],
                            IdCurso = (int)reader["IdCurso"],
                            IdProfesor = (int)reader["IdProfesor"]
                        };

                        cursos.Add(tarea);
                    }

                    return cursos;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener las tareas de la base de datos.", ex);
                }
            }
        }

        public static IEnumerable<Tarea> ObtenerTareasDeEstudiante(int idEstudiante)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT t.*, c.NombreCurso
                FROM Tarea t
                INNER JOIN Curso c ON t.IdCurso = c.IdCurso
                INNER JOIN TareaEstudiante te ON t.IdTarea = te.IdTarea
                WHERE te.IdEstudiante = @IdEstudiante";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                List<Tarea> tareas = new List<Tarea>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea
                        {
                            IdTarea = (int)reader["IdTarea"],
                            NombreTarea = (string)reader["NombreTarea"],
                            Descripcion = (string)reader["Descripcion"],
                            FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd"),
                            FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd"),
                            DocumentoRuta = (string)reader["DocumentoRuta"],
                            IdCurso = (int)reader["IdCurso"],
                            IdProfesor = (int)reader["IdProfesor"],
                            NombreCurso = (string)reader["NombreCurso"] 
                        };

                        tareas.Add(tarea);
                    }

                    return tareas;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }
            }
        }

        public static void CrearTarea(Tarea tarea, int idCurso, int idProfesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                DECLARE @IdTarea INT;

                -- Insertar nueva tarea
                INSERT INTO Tarea (NombreTarea, Descripcion, FechaLimite, DocumentoRuta, IdCurso, FechaInicio, IdProfesor)
                VALUES (@NombreTarea, @Descripcion, @FechaLimite, @DocumentoRuta, @idCurso, GETDATE(), @idProfesor);

                -- Obtener el Id de la tarea recién insertada
                SET @IdTarea = SCOPE_IDENTITY();

                -- Insertar la tarea para todos los estudiantes inscritos en el curso
                INSERT INTO TareaEstudiante (IdTarea, IdEstudiante, FechaEntrega, DocuEntregadoRuta)
                SELECT @IdTarea, IdEstudiante, '1900-01-01', ''
                FROM InscripcionesCurso
                WHERE IdCurso = @idCurso;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NombreTarea", tarea.NombreTarea);
                command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
                command.Parameters.AddWithValue("@FechaLimite", tarea.FechaLimite);
                command.Parameters.AddWithValue("@DocumentoRuta", tarea.DocumentoRuta);
                command.Parameters.AddWithValue("@idCurso", idCurso);
                command.Parameters.AddWithValue("@idProfesor", idProfesor);
                
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al crear la tarea en la base de datos.", ex);
                }
            }
        }

        internal static IEnumerable<Tarea> TareasDeCurso(int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT t.*, c.NombreCurso
                FROM Tarea t
                INNER JOIN Curso c ON t.IdCurso = c.IdCurso
                WHERE c.IdCurso = @IdCurso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdCurso", idCurso);
                List<Tarea> tareas = new List<Tarea>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea
                        {
                            IdTarea = (int)reader["IdTarea"],
                            NombreTarea = (string)reader["NombreTarea"],
                            Descripcion = (string)reader["Descripcion"],
                            FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd"),
                            FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd"),
                            DocumentoRuta = (string)reader["DocumentoRuta"],
                            IdCurso = (int)reader["IdCurso"],
                            IdProfesor = (int)reader["IdProfesor"],
                            NombreCurso = (string)reader["NombreCurso"]
                        };

                        tareas.Add(tarea);
                    }

                    return tareas;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las tareas de la base de datos.", ex);
                }
            }
        }

        internal static IEnumerable<Tarea> TareasDeCursoDeEstudiante(int idEstudiante, int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT t.*, c.NombreCurso
                FROM Tarea t
                INNER JOIN Curso c ON t.IdCurso = c.IdCurso
                INNER JOIN TareaEstudiante te ON t.IdTarea = te.IdTarea
                WHERE te.IdEstudiante = @IdEstudiante
                AND c.IdCurso = @idCurso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@IdCurso", idCurso);
                List<Tarea> tareas = new List<Tarea>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea
                        {
                            IdTarea = (int)reader["IdTarea"],
                            NombreTarea = (string)reader["NombreTarea"],
                            Descripcion = (string)reader["Descripcion"],
                            FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd"),
                            FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd"),
                            DocumentoRuta = (string)reader["DocumentoRuta"],
                            IdCurso = (int)reader["IdCurso"],
                            IdProfesor = (int)reader["IdProfesor"],
                            NombreCurso = (string)reader["NombreCurso"]
                        };

                        tareas.Add(tarea);
                    }

                    return tareas;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los cursos de la base de datos.", ex);
                }
            }
        }
    }
}