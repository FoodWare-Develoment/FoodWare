using FoodWare.Model.Entities;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    // Clase responsable de la conexión de la tabla MENU_ITEMS y las operaciones CRUD con la base de datos.
    public class ConexionMenuItem
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<MenuItem> ObtenerTodosMenuItems()
        {
            string query = "SELECT id_item, nombre, tipo, descripcion, precio, moneda, gramaje_o_ml FROM db_foodware1.MENU_ITEMS;";
            List<MenuItem> lista = new List<MenuItem>();

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
                            int descripcionIndex = reader.GetOrdinal("descripcion");
                            int gramajeIndex = reader.GetOrdinal("gramaje_o_ml");

                            lista.Add(new MenuItem
                            {
                                IdItem = reader.GetInt32("id_item"),
                                Nombre = reader.GetString("nombre"),
                                Tipo = reader.GetString("tipo"),
                                Descripcion = reader.IsDBNull(descripcionIndex) ? string.Empty : reader.GetString(descripcionIndex),
                                Precio = reader.GetDecimal("precio"),
                                Moneda = reader.GetString("moneda"),
                                GramajeOMl = reader.IsDBNull(gramajeIndex) ? (decimal?)null : reader.GetDecimal(gramajeIndex)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer items del menú: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearMenuItem(MenuItem nuevo)
        {
            string query = "INSERT INTO db_foodware1.MENU_ITEMS (nombre, tipo, descripcion, precio, moneda, gramaje_o_ml) VALUES (@Nombre, @Tipo, @Desc, @Precio, @Moneda, @Gramaje)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
                command.Parameters.AddWithValue("@Tipo", nuevo.Tipo);
                command.Parameters.AddWithValue("@Desc", string.IsNullOrEmpty(nuevo.Descripcion) ? (object)DBNull.Value : nuevo.Descripcion);
                command.Parameters.AddWithValue("@Precio", nuevo.Precio);
                command.Parameters.AddWithValue("@Moneda", nuevo.Moneda);
                command.Parameters.AddWithValue("@Gramaje", nuevo.GramajeOMl ?? (object)DBNull.Value);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear item del menú: " + ex.Message);
                    return false;
                }
            }
        }

        // ... (U y D siguen el mismo patrón)
    }
}
