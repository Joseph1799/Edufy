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
                            NombreCurso = (string)reader["NombreCurso"] // Agregar el nombre del curso
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