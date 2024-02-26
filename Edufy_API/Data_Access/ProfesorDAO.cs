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

        public static bool Login(string correoElectronico, string contrasenia, string rol)
        {
            string connectionString = Connection.connectionRoute;
            string query = "SELECT COUNT(1) FROM Profesor WHERE CorreoElectronico = @CorreoElectronico AND Contrasenia = @Contrasenia AND Rol = @Rol";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CorreoElectronico", correoElectronico);
                command.Parameters.AddWithValue("@Contrasenia", contrasenia);
                command.Parameters.AddWithValue("@Rol", rol);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
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