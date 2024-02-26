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

        public static bool Login(string correoElectronico, string contrasenia, string rol)
        {
            string connectionString = Connection.connectionRoute;
            string query = "SELECT COUNT(1) FROM Estudiante WHERE CorreoElectronico = @CorreoElectronico AND Contrasenia = @Contrasenia AND Rol = @Rol";

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
    