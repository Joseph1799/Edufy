using Edufy_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Edufy_API.Data_Access
{
    public class ProfesorDAO
    {

        public static bool Registrar(Profesor oProfesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = "INSERT INTO Profesor (Id, Nombre, Apellido, CorreoElectronico, Contrasenia, FechaNacimiento, Genero, Departamento, TituloAcademico, EstadoCuenta, FotoPerfil, Rol) " +
                           "VALUES (@Id, @Nombre, @Apellido, @CorreoElectronico, @Contrasenia, @FechaNacimiento, @Genero, @Departamento, @TituloAcademico, @EstadoCuenta, @FotoPerfil, @Rol)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", oProfesor.Id);
                command.Parameters.AddWithValue("@Nombre", oProfesor.Nombre);
                command.Parameters.AddWithValue("@Apellido", oProfesor.Apellido);
                command.Parameters.AddWithValue("@CorreoElectronico", oProfesor.CorreoElectronico);
                command.Parameters.AddWithValue("@Contrasenia", oProfesor.Contrasenia);
                command.Parameters.AddWithValue("@FechaNacimiento", oProfesor.FechaNacimiento);
                command.Parameters.AddWithValue("@Genero", oProfesor.Genero);
                command.Parameters.AddWithValue("@Departamento", oProfesor.Departamento);
                command.Parameters.AddWithValue("@TituloAcademico", oProfesor.TituloAcademico);
                command.Parameters.AddWithValue("@EstadoCuenta", oProfesor.EstadoCuenta);
                command.Parameters.AddWithValue("@FotoPerfil", oProfesor.FotoPerfil);
                command.Parameters.AddWithValue("@Rol", oProfesor.Rol);

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

        public static Profesor Login(string correoElectronico, string contrasenia, string rol)
        {
            string connectionString = Connection.connectionRoute;
            string query = "SELECT Id, CorreoElectronico, Rol FROM Profesor WHERE CorreoElectronico = @CorreoElectronico AND Contrasenia = @Contrasenia AND Rol = @Rol";

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
                        Profesor profesor = new Profesor
                        {
                            Id = (int)reader["Id"],
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            Rol = reader["Rol"].ToString(),
                        };
                        return profesor;
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


        public static Profesor GetProfesorById(int id)
        {
            string connectionString = Connection.connectionRoute;
            string query = "SELECT * FROM Profesor WHERE Id = @Id";

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
                        Profesor profesor = new Profesor
                        {
                            Id = (int)reader["Id"],
                            Nombre = (string)reader["Nombre"],
                            Apellido = (string)reader["Apellido"],
                            CorreoElectronico = (string)reader["CorreoElectronico"],
                            Contrasenia = (string)reader["Contrasenia"],
                            FechaNacimiento = ((DateTime)reader["FechaNacimiento"]).ToString("yyyy-MM-dd"),
                            Genero = (string)reader["Genero"],
                            Departamento = (string)reader["Departamento"],
                            TituloAcademico = (string)reader["TituloAcademico"],
                            EstadoCuenta = (string)reader["EstadoCuenta"],
                            FotoPerfil = (string)reader["FotoPerfil"],
                            Rol = (string)reader["Rol"],
                        };
                        return profesor;
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


        public static bool ModificarProfesor(Profesor profesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"UPDATE Profesor SET Nombre = @Nombre, Apellido = @Apellido,
            CorreoElectronico = @CorreoElectronico, FechaNacimiento = @FechaNacimiento,
            Genero = @Genero, Departamento = @Departamento, TituloAcademico = @TituloAcademico 
            WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", profesor.Id);
                command.Parameters.AddWithValue("@Nombre", profesor.Nombre);
                command.Parameters.AddWithValue("@Apellido", profesor.Apellido);
                command.Parameters.AddWithValue("@CorreoElectronico", profesor.CorreoElectronico);
                command.Parameters.AddWithValue("@FechaNacimiento", profesor.FechaNacimiento);
                command.Parameters.AddWithValue("@Genero", profesor.Genero);
                command.Parameters.AddWithValue("@Departamento", profesor.Departamento);
                command.Parameters.AddWithValue("@TituloAcademico", profesor.TituloAcademico);
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

        public static bool ModificarContrasenia(Profesor profesor)
        {
            string connectionString = Connection.connectionRoute;
            string query = @"UPDATE Profesor SET Contrasenia = @Contrasenia WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", profesor.Id);
                command.Parameters.AddWithValue("@Contrasenia", profesor.Contrasenia);

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

