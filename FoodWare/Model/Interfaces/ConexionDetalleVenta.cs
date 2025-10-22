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
    // Clase responsable de la conexión de la tabla DETALLE_VENTA y las operaciones CRUD con la base de datos.
    public class ConexionDetalleVenta
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<DetalleVenta> ObtenerTodosDetallesVenta()
        {
            string query = "SELECT id_detalle, id_venta, id_item, cantidad, precio_unitario_venta FROM db_foodware1.DETALLE_VENTA;";
            List<DetalleVenta> lista = new List<DetalleVenta>();

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
                            lista.Add(new DetalleVenta
                            {
                                IdDetalle = reader.GetInt32("id_detalle"),
                                IdVenta = reader.GetInt32("id_venta"),
                                IdItem = reader.GetInt32("id_item"),
                                Cantidad = reader.GetInt32("cantidad"),
                                PrecioUnitarioVenta = reader.GetDecimal("precio_unitario_venta")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer detalles de venta: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearDetalleVenta(DetalleVenta nuevo)
        {
            string query = "INSERT INTO db_foodware1.DETALLE_VENTA (id_venta, id_item, cantidad, precio_unitario_venta) VALUES (@IdVenta, @IdItem, @Cantidad, @Precio)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdVenta", nuevo.IdVenta);
                command.Parameters.AddWithValue("@IdItem", nuevo.IdItem);
                command.Parameters.AddWithValue("@Cantidad", nuevo.Cantidad);
                command.Parameters.AddWithValue("@Precio", nuevo.PrecioUnitarioVenta);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear detalle de venta: " + ex.Message);
                    return false;
                }
            }
        }

        // ... (U y D siguen el mismo patrón)
    }
}
