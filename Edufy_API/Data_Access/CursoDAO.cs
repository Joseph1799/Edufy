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

    }
}