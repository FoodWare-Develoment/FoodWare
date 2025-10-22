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
    // Clase responsable de la conexión de la tabla VENTAS y las operaciones CRUD con la base de datos.
    public class ConexionVenta
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<Venta> ObtenerTodasVentas()
        {
            string query = "SELECT id_venta, fecha_cobro, forma_pago, monto_total, moneda, id_mesero, id_chef FROM db_foodware1.VENTAS;";
            List<Venta> lista = new List<Venta>();

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
                            int idMeseroIndex = reader.GetOrdinal("id_mesero");
                            int idChefIndex = reader.GetOrdinal("id_chef");

                            lista.Add(new Venta
                            {
                                IdVenta = reader.GetInt32("id_venta"),
                                FechaCobro = reader.GetDateTime("fecha_cobro"),
                                FormaPago = reader.GetString("forma_pago"),
                                MontoTotal = reader.GetDecimal("monto_total"),
                                Moneda = reader.GetString("moneda"),

                                IdMesero = reader.IsDBNull(idMeseroIndex) ? (int?)null : reader.GetInt32(idMeseroIndex),
                                IdChef = reader.IsDBNull(idChefIndex) ? (int?)null : reader.GetInt32(idChefIndex)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer ventas: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearVenta(Venta nuevaVenta)
        {
            // Omitimos fecha_cobro para que use el DEFAULT (CURRENT_TIMESTAMP) de la BD si no se especifica.
            string query = "INSERT INTO db_foodware1.VENTAS (forma_pago, monto_total, moneda, id_mesero, id_chef) VALUES (@FPago, @Monto, @Moneda, @IdMesero, @IdChef)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FPago", nuevaVenta.FormaPago);
                command.Parameters.AddWithValue("@Monto", nuevaVenta.MontoTotal);
                command.Parameters.AddWithValue("@Moneda", nuevaVenta.Moneda);
                command.Parameters.AddWithValue("@IdMesero", nuevaVenta.IdMesero ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IdChef", nuevaVenta.IdChef ?? (object)DBNull.Value);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear venta: " + ex.Message);
                    return false;
                }
            }
        }

        // ... (U y D siguen el mismo patrón)
    }
}