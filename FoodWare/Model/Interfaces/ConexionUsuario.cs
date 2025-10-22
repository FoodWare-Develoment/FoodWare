using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using FoodWare.Model.Entities;

namespace FoodWare.Model.Interfaces
{
    // Clase responsable de la conexión de la tabla USUARIOS y las operaciones CRUD con la base de datos.
    public class ConexionUsuario
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<Usuario> ObtenerTodosUsuarios()
        {
            string query = "SELECT id_usuario, username, password_hash, id_empleado, es_admin FROM db_foodware1.USUARIOS;";
            List<Usuario> lista = new List<Usuario>();

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
                            int idEmpleadoIndex = reader.GetOrdinal("id_empleado");

                            lista.Add(new Usuario
                            {
                                IdUsuario = reader.GetInt32("id_usuario"),
                                Username = reader.GetString("username"),
                                PasswordHash = reader.GetString("password_hash"),
                                IdEmpleado = reader.IsDBNull(idEmpleadoIndex) ? (int?)null : reader.GetInt32(idEmpleadoIndex), // id_empleado es NULLable
                                EsAdmin = reader.GetBoolean("es_admin")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer usuarios: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            string query = "INSERT INTO db_foodware1.USUARIOS (username, password_hash, id_empleado, es_admin) VALUES (@Username, @PassHash, @IdEmp, @Admin)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", nuevoUsuario.Username);
                command.Parameters.AddWithValue("@PassHash", nuevoUsuario.PasswordHash);
                command.Parameters.AddWithValue("@IdEmp", nuevoUsuario.IdEmpleado ?? (object)DBNull.Value); // Manejo de int? (IdEmpleado)
                command.Parameters.AddWithValue("@Admin", nuevoUsuario.EsAdmin);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear usuario: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN U (UPDATE / ACTUALIZAR)
        // =================================================================
        public bool ActualizarUsuario(Usuario usuarioAActualizar)
        {
            string query = "UPDATE db_foodware1.USUARIOS SET username = @Username, password_hash = @PassHash, id_empleado = @IdEmp, es_admin = @Admin WHERE id_usuario = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", usuarioAActualizar.Username);
                command.Parameters.AddWithValue("@PassHash", usuarioAActualizar.PasswordHash);
                command.Parameters.AddWithValue("@IdEmp", usuarioAActualizar.IdEmpleado ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Admin", usuarioAActualizar.EsAdmin);
                command.Parameters.AddWithValue("@Id", usuarioAActualizar.IdUsuario);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar usuario: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN D (DELETE / ELIMINAR)
        // =================================================================
        public bool EliminarUsuario(int idUsuario)
        {
            string query = "DELETE FROM db_foodware1.USUARIOS WHERE id_usuario = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", idUsuario);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
