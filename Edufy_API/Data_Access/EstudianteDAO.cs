using Edufy_API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Edufy_API.Data_Access
{
    public class EstudianteDAO
    {

        public static bool Registrar(Estudiante oEstudiante)
        {
            string connectionString = Connection.connectionRoute;
            string query = "INSERT INTO Estudiante (Id, Nombre, Apellido, CorreoElectronico, Contrasenia, FechaNacimiento, Genero, Carrera, AnioIngreso, EstadoCuenta, FotoPerfil, Rol) " +
                           "VALUES (@Id, @Nombre, @Apellido, @CorreoElectronico, @Contrasenia, @FechaNacimiento, @Genero, @Carrera, @AnioIngreso, @EstadoCuenta, @FotoPerfil, @Rol)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", oEstudiante.Id);
                command.Parameters.AddWithValue("@Nombre", oEstudiante.Nombre);
                command.Parameters.AddWithValue("@Apellido", oEstudiante.Apellido);
                command.Parameters.AddWithValue("@CorreoElectronico", oEstudiante.CorreoElectronico);
                command.Parameters.AddWithValue("@Contrasenia", oEstudiante.Contrasenia);
                command.Parameters.AddWithValue("@FechaNacimiento", oEstudiante.FechaNacimiento);
                command.Parameters.AddWithValue("@Genero", oEstudiante.Genero);
                command.Parameters.AddWithValue("@Carrera", oEstudiante.Carrera);
                command.Parameters.AddWithValue("@AnioIngreso", oEstudiante.AnioIngreso);
                command.Parameters.AddWithValue("@EstadoCuenta", oEstudiante.EstadoCuenta);
                command.Parameters.AddWithValue("@FotoPerfil", oEstudiante.FotoPerfil);
                command.Parameters.AddWithValue("@Rol", oEstudiante.Rol);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return false;
                }
            }
        }

        public static Estudiante Login(string correoElectronico, string contrasenia, string rol)
        {
            string connectionString = Connection.connectionRoute;
            string query = "SELECT Id FROM Estudiante WHERE CorreoElectronico = @CorreoElectronico AND Contrasenia = @Contrasenia AND Rol = @Rol";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CorreoElectronico", correoElectronico);
                command.Parameters.AddWithValue("@Contrasenia", contrasenia);
                command.Parameters.AddWithValue("@Rol", rol);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        return new Estudiante { Id = id };
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return null;
                }
            }
        }



        public static Estudiante GetEstudianteById(int id)
        {
            string connectionString = Connection.connectionRoute;
            string query = "SELECT * FROM Estudiante WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Crear un objeto Estudiante con los datos de la base de datos
                        Estudiante estudiante = new Estudiante
                        {
                            Id = (int)reader["Id"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            CorreoElectronico = (string)reader["CorreoElectronico"],
                            Contrasenia = (string)reader["Contrasenia"],
                            FechaNacimiento = ((DateTime)reader["FechaNacimiento"]).ToString("yyyy-MM-dd"),
                            Genero = (string)reader["Genero"],
                            Carrera = (string)reader["Carrera"],
                            AnioIngreso = (int)reader["AnioIngreso"],
                            EstadoCuenta = (string)reader["EstadoCuenta"],
                            FotoPerfil = (string)reader["FotoPerfil"],
                            Rol = (string)reader["Rol"],
                        };
                        return estudiante;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return null;
                }
            }
        }

        public static bool Modificar(Estudiante estudiante)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"UPDATE Estudiante SET Contrasenia = @Contrasenia WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", estudiante.Id);
                command.Parameters.AddWithValue("@Contrasenia", estudiante.Contrasenia);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return false;
                }
            }
        }


    }
}
    