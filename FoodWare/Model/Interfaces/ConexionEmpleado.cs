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
    // Clase responsable de la conexión de la tabla EMPLEADOS y las operaciones CRUD con la base de datos.
    public class ConexionEmpleado
    {
        // 1. Configura tu cadena de conexión usando el Endpoint de AWS RDS
        // ¡IMPORTANTE! Reemplaza los placeholders con tus datos reales de AWS.
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";
        // MI SERVER
        // "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";


        // Método auxiliar para obtener la conexión
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // =================================================================
        // OPERACIÓN R (READ / LEER) - CORRECCIÓN DE NULOS
        // =================================================================
        public List<Empleado> ObtenerTodosEmpleados()
        {
            string query = "SELECT id_empleado, nombre, apellido_pat, apellido_mat, sueldo, fecha_contrato_inicio, fecha_contrato_final, id_rol FROM db_foodware1.EMPLEADOS;";

            List<Empleado> listaEmpleados = new List<Empleado>();

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
                            // Lógica para manejar valores NULL en C#
                            // Usamos el índice de la columna (GetOrdinal) para verificar si es nulo.

                            // Captura las posiciones de las columnas nulas solo una vez
                            int finalContractIndex = reader.GetOrdinal("fecha_contrato_final");
                            int apellidoMatIndex = reader.GetOrdinal("apellido_mat");

                            listaEmpleados.Add(new Empleado
                            {
                                IdEmpleado = reader.GetInt32("id_empleado"),
                                Nombre = reader.GetString("nombre"),

                                // Apellido Paterno: Asumiendo que es NOT NULL
                                ApellidoPaterno = reader.GetString("apellido_pat"),

                                // Apellido Materno: Verifica si es nulo (devuelve string.Empty si lo es)
                                ApellidoMaterno = reader.IsDBNull(apellidoMatIndex) ? string.Empty : reader.GetString(apellidoMatIndex),

                                // Sueldo: Asumiendo que es NOT NULL, pero si permitiera NULL, se haría la verificación similar.
                                Sueldo = reader.GetDecimal("sueldo"),

                                FechaContratoInicio = reader.GetDateTime("fecha_contrato_inicio"),

                                // FechaContratoFinal: Verifica si es nulo y lo asigna a un DateTime? (nullable)
                                FechaContratoFinal = reader.IsDBNull(finalContractIndex) ? (DateTime?)null : reader.GetDateTime(finalContractIndex),

                                IdRol = reader.GetInt32("id_rol")
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Esto captura errores de conexión o SQL
                    Console.WriteLine("Error de MySql al leer empleados: " + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    // Esto captura errores de lectura de datos (como el SqlNullValueException)
                    Console.WriteLine("Error de lectura de datos: " + ex.Message);
                    return null;
                }
            }
            return listaEmpleados;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR)
        // =================================================================
        public bool CrearEmpleado(Empleado nuevoEmpleado)
        {
            // Nota: Aquí también se incluye el esquema 'db_foodware1.' para asegurar la tabla correcta.
            string query = "INSERT INTO db_foodware1.EMPLEADOS (nombre, apellido_pat, apellido_mat, sueldo, fecha_contrato_inicio, id_rol) VALUES (@Nombre, @APat, @AMat, @Sueldo, @FechaInicio, @IdRol)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Uso de parámetros para prevenir inyección SQL
                command.Parameters.AddWithValue("@Nombre", nuevoEmpleado.Nombre);
                command.Parameters.AddWithValue("@APat", nuevoEmpleado.ApellidoPaterno);
                command.Parameters.AddWithValue("@AMat", nuevoEmpleado.ApellidoMaterno);
                command.Parameters.AddWithValue("@Sueldo", nuevoEmpleado.Sueldo);
                command.Parameters.AddWithValue("@FechaInicio", nuevoEmpleado.FechaContratoInicio);
                command.Parameters.AddWithValue("@IdRol", nuevoEmpleado.IdRol);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al crear empleado: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN U (UPDATE / ACTUALIZAR)
        // =================================================================
        public bool ActualizarEmpleado(Empleado empleadoAActualizar)
        {
            string query = "UPDATE db_foodware1.EMPLEADOS SET nombre = @Nombre, sueldo = @Sueldo, id_rol = @IdRol WHERE id_empleado = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Parámetros
                command.Parameters.AddWithValue("@Nombre", empleadoAActualizar.Nombre);
                command.Parameters.AddWithValue("@Sueldo", empleadoAActualizar.Sueldo);
                command.Parameters.AddWithValue("@IdRol", empleadoAActualizar.IdRol);
                command.Parameters.AddWithValue("@Id", empleadoAActualizar.IdEmpleado);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al actualizar empleado: " + ex.Message);
                    return false;
                }
            }
        }

        // =================================================================
        // OPERACIÓN D (DELETE / ELIMINAR)
        // =================================================================
        public bool EliminarEmpleado(int idEmpleado)
        {
            string query = "DELETE FROM db_foodware1.EMPLEADOS WHERE id_empleado = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Parámetro
                command.Parameters.AddWithValue("@Id", idEmpleado);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() == 1;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error al eliminar empleado: " + ex.Message);
                    return false;
                }
            }
        }
    }
}


