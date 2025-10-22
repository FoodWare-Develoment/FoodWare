using FoodWare.Model.Entities; // Asegura que se usa la entidad correcta
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace FoodWare.Model.Interfaces
{
    // Clase responsable de la conexión de la tabla ROL y las operaciones CRUD con la base de datos.
    public class ConexionRol
    {
        // 1. Configura tu cadena de conexión usando el Endpoint de AWS RDS
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";

        // Método auxiliar para obtener la conexión
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<Rol> ObtenerTodosRoles()
        {
            string query = "SELECT id_rol, categoria FROM db_foodware1.ROL;";
            List<Rol> listaRoles = new List<Rol>();

            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaRoles.Add(new Rol
                            {
                                IdRol = reader.GetInt32("id_rol"),
                                Categoria = reader.GetString("categoria")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer roles: " + ex.Message);
                    return null;
                }
            }
            return listaRoles;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearRol(Rol nuevoRol)
        {
            string query = "INSERT INTO db_foodware1.ROL (categoria) VALUES (@Categoria)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Categoria", nuevoRol.Categoria);
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear rol: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN U (UPDATE / ACTUALIZAR)
        // =================================================================
        public bool ActualizarRol(Rol rolAActualizar)
        {
            string query = "UPDATE db_foodware1.ROL SET categoria = @Categoria WHERE id_rol = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Categoria", rolAActualizar.Categoria);
                command.Parameters.AddWithValue("@Id", rolAActualizar.IdRol);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar rol: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN D (DELETE / ELIMINAR)
        // =================================================================
        public bool EliminarRol(int idRol)
        {
            string query = "DELETE FROM db_foodware1.ROL WHERE id_rol = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", idRol);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar rol: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
