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
    // Clase responsable de la conexión de la tabla FACTURAS y las operaciones CRUD con la base de datos.
    public class ConexionFactura
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<Factura> ObtenerTodasFacturas()
        {
            string query = "SELECT id_factura, id_venta, nombre_factura, rfc, forma_pago_sat, uso_cfdi, monto_facturado, regimen_fiscal, fecha_emision FROM db_foodware1.FACTURAS;";
            List<Factura> lista = new List<Factura>();

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
                            int regimenFiscalIndex = reader.GetOrdinal("regimen_fiscal");

                            lista.Add(new Factura
                            {
                                IdFactura = reader.GetInt32("id_factura"),
                                IdVenta = reader.GetInt32("id_venta"),
                                NombreFactura = reader.GetString("nombre_factura"),
                                Rfc = reader.GetString("rfc"),
                                FormaPagoSat = reader.GetString("forma_pago_sat"),
                                UsoCfdi = reader.GetString("uso_cfdi"),
                                MontoFacturado = reader.GetDecimal("monto_facturado"),
                                RegimenFiscal = reader.IsDBNull(regimenFiscalIndex) ? string.Empty : reader.GetString(regimenFiscalIndex),
                                FechaEmision = reader.GetDateTime("fecha_emision")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer facturas: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearFactura(Factura nueva)
        {
            // Omitimos fecha_emision para que use el DEFAULT (CURRENT_TIMESTAMP) de la BD si no se especifica.
            string query = "INSERT INTO db_foodware1.FACTURAS (id_venta, nombre_factura, rfc, forma_pago_sat, uso_cfdi, monto_facturado, regimen_fiscal) VALUES (@IdVenta, @Nombre, @Rfc, @FormaPago, @UsoCfdi, @Monto, @Regimen)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdVenta", nueva.IdVenta);
                command.Parameters.AddWithValue("@Nombre", nueva.NombreFactura);
                command.Parameters.AddWithValue("@Rfc", nueva.Rfc);
                command.Parameters.AddWithValue("@FormaPago", nueva.FormaPagoSat);
                command.Parameters.AddWithValue("@UsoCfdi", nueva.UsoCfdi);
                command.Parameters.AddWithValue("@Monto", nueva.MontoFacturado);
                command.Parameters.AddWithValue("@Regimen", string.IsNullOrEmpty(nueva.RegimenFiscal) ? (object)DBNull.Value : nueva.RegimenFiscal);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear factura: " + ex.Message);
                    return false;
                }
            }
        }

        // ... (U y D siguen el mismo patrón)
    }
}
