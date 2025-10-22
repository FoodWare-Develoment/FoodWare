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
    // Clase responsable de la conexión de la tabla PRODUCTOS y las operaciones CRUD con la base de datos.
    public class ConexionProducto
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<Producto> ObtenerTodosProductos()
        {
            string query = "SELECT id_producto, nombre, unidad_medida, stock_actual, precio_compra, fecha_compra, fecha_caducidad FROM db_foodware1.PRODUCTOS;";
            List<Producto> lista = new List<Producto>();

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
                            // Índices de columnas NULLable:
                            int precioCompraIndex = reader.GetOrdinal("precio_compra");
                            int fechaCompraIndex = reader.GetOrdinal("fecha_compra");
                            int fechaCaducidadIndex = reader.GetOrdinal("fecha_caducidad");

                            lista.Add(new Producto
                            {
                                IdProducto = reader.GetInt32("id_producto"),
                                Nombre = reader.GetString("nombre"),
                                UnidadMedida = reader.GetString("unidad_medida"),
                                StockActual = reader.GetDecimal("stock_actual"),

                                PrecioCompra = reader.IsDBNull(precioCompraIndex) ? (decimal?)null : reader.GetDecimal(precioCompraIndex),
                                FechaCompra = reader.IsDBNull(fechaCompraIndex) ? (DateTime?)null : reader.GetDateTime(fechaCompraIndex),
                                FechaCaducidad = reader.IsDBNull(fechaCaducidadIndex) ? (DateTime?)null : reader.GetDateTime(fechaCaducidadIndex)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer productos: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearProducto(Producto nuevo)
        {
            string query = "INSERT INTO db_foodware1.PRODUCTOS (nombre, unidad_medida, stock_actual, precio_compra, fecha_compra, fecha_caducidad) VALUES (@Nombre, @UMedida, @Stock, @PCompra, @FCompra, @FCad)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
                command.Parameters.AddWithValue("@UMedida", nuevo.UnidadMedida);
                command.Parameters.AddWithValue("@Stock", nuevo.StockActual);

                // Manejo de NULLable decimal? y DateTime?
                command.Parameters.AddWithValue("@PCompra", nuevo.PrecioCompra ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FCompra", nuevo.FechaCompra ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FCad", nuevo.FechaCaducidad ?? (object)DBNull.Value);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear producto: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN U (UPDATE / ACTUALIZAR)
        // =================================================================
        public bool ActualizarProducto(Producto productoAActualizar)
        {
            string query = "UPDATE db_foodware1.PRODUCTOS SET nombre = @Nombre, unidad_medida = @UMedida, stock_actual = @Stock, precio_compra = @PCompra, fecha_compra = @FCompra, fecha_caducidad = @FCad WHERE id_producto = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", productoAActualizar.Nombre);
                command.Parameters.AddWithValue("@UMedida", productoAActualizar.UnidadMedida);
                command.Parameters.AddWithValue("@Stock", productoAActualizar.StockActual);
                command.Parameters.AddWithValue("@PCompra", productoAActualizar.PrecioCompra ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FCompra", productoAActualizar.FechaCompra ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FCad", productoAActualizar.FechaCaducidad ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Id", productoAActualizar.IdProducto);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar producto: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN D (DELETE / ELIMINAR)
        // =================================================================
        public bool EliminarProducto(int idProducto)
        {
            string query = "DELETE FROM db_foodware1.PRODUCTOS WHERE id_producto = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", idProducto);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar producto: " + ex.Message);
                    return false;
                }
            }
        }
    }
}

