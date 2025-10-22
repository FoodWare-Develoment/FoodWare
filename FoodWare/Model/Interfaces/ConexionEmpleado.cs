using FoodWare.Model.Entities;
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
        private readonly string connectionString = "server=db-foodware-1.ch60qia0smb4.us-east-2.rds.amazonaws.com;database=db_foodware1;uid=admin;pwd=4+67.(=^fGw";

        // Método auxiliar para obtener la conexión
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // =================================================================
        // OPERACIÓN R (READ / LEER) - Lógica de Nulos C->BD OK
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
                        // Captura las posiciones de las columnas nulas solo una vez
                        int finalContractIndex = reader.GetOrdinal("fecha_contrato_final");
                        int apellidoMatIndex = reader.GetOrdinal("apellido_mat");

                        while (reader.Read())
                        {
                            listaEmpleados.Add(new Empleado
                            {
                                IdEmpleado = reader.GetInt32("id_empleado"),
                                Nombre = reader.GetString("nombre"),
                                ApellidoPaterno = reader.GetString("apellido_pat"),

                                // Apellido Materno: Verifica si es nulo (devuelve string.Empty si lo es)
                                ApellidoMaterno = reader.IsDBNull(apellidoMatIndex) ? string.Empty : reader.GetString(apellidoMatIndex),

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
                    Console.WriteLine("Error de MySql al leer empleados: " + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error de lectura de datos: " + ex.Message);
                    return null;
                }
            }
            return listaEmpleados;
        }

        // =================================================================
        // OPERACIÓN C (CREATE / CREAR) - CORRECCIÓN DE NULOS HACIA BD
        // =================================================================
        public bool CrearEmpleado(Empleado nuevoEmpleado)
        {
            // Se incluye fecha_contrato_final en el insert si es que el empleado tiene un contrato final, sino, se usa un valor null.
            string query = "INSERT INTO db_foodware1.EMPLEADOS (nombre, apellido_pat, apellido_mat, sueldo, fecha_contrato_inicio, fecha_contrato_final, id_rol) VALUES (@Nombre, @APat, @AMat, @Sueldo, @FechaInicio, @FechaFinal, @IdRol)";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", nuevoEmpleado.Nombre);
                command.Parameters.AddWithValue("@APat", nuevoEmpleado.ApellidoPaterno);

                // Manejo de ApellidoMaterno (string)
                command.Parameters.AddWithValue("@AMat", string.IsNullOrEmpty(nuevoEmpleado.ApellidoMaterno) ? (object)DBNull.Value : nuevoEmpleado.ApellidoMaterno);

                command.Parameters.AddWithValue("@Sueldo", nuevoEmpleado.Sueldo);
                command.Parameters.AddWithValue("@FechaInicio", nuevoEmpleado.FechaContratoInicio);

                // Manejo de FechaContratoFinal (DateTime?)
                command.Parameters.AddWithValue("@FechaFinal", nuevoEmpleado.FechaContratoFinal.HasValue ? (object)nuevoEmpleado.FechaContratoFinal.Value : DBNull.Value);

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
        // OPERACIÓN U (UPDATE / ACTUALIZAR) - CORRECCIÓN DE NULOS HACIA BD
        // =================================================================
        public bool ActualizarEmpleado(Empleado empleadoAActualizar)
        {
            // Incluye apellido_pat y las fechas para un update completo
            string query = "UPDATE db_foodware1.EMPLEADOS SET nombre = @Nombre, apellido_pat = @APat, apellido_mat = @AMat, sueldo = @Sueldo, fecha_contrato_inicio = @FechaInicio, fecha_contrato_final = @FechaFinal, id_rol = @IdRol WHERE id_empleado = @Id;";

            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nombre", empleadoAActualizar.Nombre);
                command.Parameters.AddWithValue("@APat", empleadoAActualizar.ApellidoPaterno);

                // Manejo de ApellidoMaterno (string)
                command.Parameters.AddWithValue("@AMat", string.IsNullOrEmpty(empleadoAActualizar.ApellidoMaterno) ? (object)DBNull.Value : empleadoAActualizar.ApellidoMaterno);

                command.Parameters.AddWithValue("@Sueldo", empleadoAActualizar.Sueldo);
                command.Parameters.AddWithValue("@FechaInicio", empleadoAActualizar.FechaContratoInicio);

                // Manejo de FechaContratoFinal (DateTime?)
                command.Parameters.AddWithValue("@FechaFinal", empleadoAActualizar.FechaContratoFinal.HasValue ? (object)empleadoAActualizar.FechaContratoFinal.Value : DBNull.Value);

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
