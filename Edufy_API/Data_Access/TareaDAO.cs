using Edufy_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
                WHERE te.IdEstudiante = @IdEstudiante
                AND te.entregada = 0";

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
                INSERT INTO TareaEstudiante (IdTarea, IdEstudiante, FechaEntrega, DocuEntregadoRuta, entregada, puntaje, revisada)
                SELECT @IdTarea, IdEstudiante, '1900-01-01', '', @entregada, 0, 0
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
                command.Parameters.AddWithValue("@entregada", 0);
                
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
                AND c.IdCurso = @idCurso
                AND te.entregada = 0";

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

        internal static void EntregarTarea(int idTarea, int idEstudiante, string docuEntregadoRuta)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                -- Actualizar la tarea para un estudiante específico
                UPDATE TareaEstudiante
                SET FechaEntrega = @FechaEntrega, DocuEntregadoRuta = @DocumentoRuta, entregada = @entregada
                WHERE IdTarea = @IdTarea AND IdEstudiante = @IdEstudiante;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DocumentoRuta", docuEntregadoRuta);
                command.Parameters.AddWithValue("@FechaEntrega", DateTime.Now.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@entregada", 1);
                command.Parameters.AddWithValue("@IdTarea", idTarea);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la tareaEstudiante en la base de datos.", ex);
                }
            }
        }

        internal static void RevisarTarea(int idEstudiante, int puntaje, int idCurso, int idProfesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                UPDATE TareaEstudiante
                SET puntaje = @puntaje, revisada = 1
                WHERE IdEstudiante = @idEstudiante
                AND entregada = 1 
                AND IdTarea IN (
                    SELECT t.IdTarea
                    FROM Tarea t
                    WHERE t.IdCurso = @idCurso
                    AND t.IdProfesor = @idProfesor
                );";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@puntaje", puntaje);
                command.Parameters.AddWithValue("@idEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@idCurso", idCurso);
                command.Parameters.AddWithValue("@idProfesor", idProfesor);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al revisar la tarea en la base de datos.", ex);
                }
            }
        }

        internal static IEnumerable<Tarea> TareasCreadasProfesor(int idProfesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT t.*,c.NombreCurso
                FROM Tarea t
                INNER JOIN Curso c ON t.IdCurso = c.IdCurso
                INNER JOIN ProfesorCurso pc ON t.IdCurso = pc.IdCurso
                WHERE pc.IdProfesor = @IdProfesor";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProfesor", idProfesor);
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

        public static IEnumerable<Tarea> TareasDeEstudianteEntregadas(int idEstudiante)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT t.*, c.NombreCurso
                FROM Tarea t
                INNER JOIN Curso c ON t.IdCurso = c.IdCurso
                INNER JOIN TareaEstudiante te ON t.IdTarea = te.IdTarea
                WHERE te.IdEstudiante = @IdEstudiante
                AND te.entregada = 1";

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

        public static IEnumerable<Object> TareasDeCursoEntregadas(int idProfesor, int idCurso)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SELECT t.*, CONCAT(e.Nombre, ' ', e.Apellido) AS NombreEstudiante
                FROM TareaEstudiante te
                INNER JOIN Tarea t ON t.IdTarea = te.IdTarea
                INNER JOIN Estudiante e ON te.IdEstudiante = e.ID
                WHERE te.entregada = 1
                AND t.IdCurso = @IdCurso
                AND t.IdProfesor = @IdProfesor";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProfesor", idProfesor);
                command.Parameters.AddWithValue("@IdCurso", idCurso);
                List<Object> tareas = new List<Object>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var tarea = new
                        {
                            IdTarea = (int)reader["IdTarea"],
                            NombreTarea = (string)reader["NombreTarea"],
                            Descripcion = (string)reader["Descripcion"],
                            FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd"),
                            FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd"),
                            DocumentoRuta = (string)reader["DocumentoRuta"],
                            IdCurso = (int)reader["IdCurso"],
                            IdProfesor = (int)reader["IdProfesor"],
                            NombreEstudiante = (string)reader["NombreEstudiante"]
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

        public static Tarea ObtenerTareaPorId(int idTarea)
        {
            string query = "SELECT * FROM Tarea WHERE IdTarea = @IdTarea";

            using (SqlConnection connection = new SqlConnection(Connection.connectionRoute))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdTarea", idTarea);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Tarea tarea = new Tarea();
                    if (reader.Read())
                    {
                        tarea.IdTarea = (int)reader["IdTarea"];
                        tarea.NombreTarea = (string)reader["NombreTarea"];
                        tarea.Descripcion = (string)reader["Descripcion"];
                        tarea.FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd");
                        tarea.FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd");
                        tarea.DocumentoRuta = (string)reader["DocumentoRuta"];
                        tarea.IdCurso = (int)reader["IdCurso"];
                        tarea.IdProfesor = (int)reader["IdProfesor"];
                    }

                    return tarea;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener las tareas de la base de datos.", ex);
                }
            }
        }

        public static Object TareaEntregadaDeCursoDeEstudiante(int idTarea, int idEstudiante, int idCurso)
        {
            string query = @"SELECT te.*
                FROM TareaEstudiante te
                INNER JOIN Tarea t ON t.IdTarea = te.IdTarea
                WHERE te.IdEstudiante = @IdEstudiante
                AND te.entregada = 1 
                AND t.IdCurso = @IdCurso
                AND t.IdTarea = @IdTarea";

            using (SqlConnection connection = new SqlConnection(Connection.connectionRoute))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdTarea", idTarea);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@IdCurso", idCurso);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    {
                        var tareaEstudiantetareaEstudiante = new
                        {
                            IdTarea = (int)reader["IdTarea"],
                            IdEstudiante = (int)reader["IdEstudiante"],
                            FechaEntrega = ((DateTime)reader["FechaEntrega"]).ToString("yyyy-MM-dd"),
                            DocuEntregadoRuta = (string)reader["DocuEntregadoRuta"],
                            entregada = (int)reader["entregada"],
                            puntaje = (int)reader["puntaje"],
                            revisada = (int)reader["revisada"]
                        };
                        return tareaEstudiantetareaEstudiante;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener las tareas de la base de datos.", ex);
                }
            }
        }

        public static void EliminarTarea(int idTarea)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"
                SET @IdTarea = (SELECT IdTarea FROM Tarea WHERE IdTarea = @idTarea);
                DELETE FROM TareaEstudiante WHERE IdTarea = @IdTarea;
                DELETE FROM Tarea WHERE IdTarea = @IdTarea;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idTarea", idTarea);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Eliminar la tarea en la base de datos.", ex);
                }
            }
        }

        public static IEnumerable<Object> TareaRevisadaDeCursoDeEstudiante(int idEstudiante, int idCurso)
        {
            Debug.WriteLine("idEstudiante: " + idEstudiante);
            Debug.WriteLine("idCurso: " + idCurso);
            string query = @"
                SELECT t.*, c.NombreCurso, te.puntaje
                FROM Tarea t
                INNER JOIN TareaEstudiante te ON t.IdTarea = te.IdTarea
                INNER JOIN Curso c ON t.IdCurso = c.IdCurso
                WHERE te.IdEstudiante = @IdEstudiante
                AND te.revisada = 1
                AND t.IdCurso = @IdCurso";

            using (SqlConnection connection = new SqlConnection(Connection.connectionRoute))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdEstudiante", idEstudiante);
                command.Parameters.AddWithValue("@IdCurso", idCurso);
                List<Object> tareas = new List<Object>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var tarea = new 
                        {
                            IdTarea = (int)reader["IdTarea"],
                            NombreTarea = (string)reader["NombreTarea"],
                            Descripcion = (string)reader["Descripcion"],
                            FechaInicio = ((DateTime)reader["FechaInicio"]).ToString("yyyy-MM-dd"),
                            FechaLimite = ((DateTime)reader["FechaLimite"]).ToString("yyyy-MM-dd"),
                            DocumentoRuta = (string)reader["DocumentoRuta"],
                            IdCurso = (int)reader["IdCurso"],
                            IdProfesor = (int)reader["IdProfesor"],
                            NombreCurso = (string)reader["NombreCurso"],
                            Puntaje = (int)reader["puntaje"]
                        };

                        tareas.Add(tarea);
                    }

                    return tareas;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    throw new Exception("Error al obtener las tareas de la base de datos.", ex);
                }
            }
        }

    }
}