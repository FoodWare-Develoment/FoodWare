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
    // Clase responsable de la conexión de la tabla RECETAS y las operaciones CRUD con la base de datos.
    public class ConexionReceta
    {
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        private MySqlConnection GetConnection() => new MySqlConnection(connectionString);

        // =================================================================
        // OPERACIÓN R (READ / LEER)
        // =================================================================
        public List<Receta> ObtenerTodasRecetas()
        {
            string query = "SELECT id_receta, id_item, id_producto, cantidad_requerida FROM db_foodware1.RECETAS;";
            List<Receta> lista = new List<Receta>();

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
                            lista.Add(new Receta
                            {
                                IdReceta = reader.GetInt32("id_receta"),
                                IdItem = reader.GetInt32("id_item"),
                                IdProducto = reader.GetInt32("id_producto"),
                                CantidadRequerida = reader.GetDecimal("cantidad_requerida")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al leer recetas: " + ex.Message);
                    return null;
                }
            }
            return lista;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearReceta(Receta nueva)
        {
            string query = "INSERT INTO db_foodware1.RECETAS (id_item, id_producto, cantidad_requerida) VALUES (@IdItem, @IdProd, @CantReq)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdItem", nueva.IdItem);
                command.Parameters.AddWithValue("@IdProd", nueva.IdProducto);
                command.Parameters.AddWithValue("@CantReq", nueva.CantidadRequerida);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear receta: " + ex.Message);
                    return false;
                }
            }
        }

        // ... (U y D siguen el mismo patrón)
    }
}
