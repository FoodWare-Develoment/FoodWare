using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FoodWare.Controller
{
    /// <summary>
    /// Clase principal que maneja la lógica de negocio (BLL) y actúa como intermediario
    /// entre la Vista (User Controls) y el Modelo (Clases de Conexión).
    /// </summary>
    public class MainController
    {
        private readonly ConexionEmpleado _conexionEmpleado;

        public MainController()
        {
            // Inicializa la clase de conexión para Empleados (la capa de acceso a datos)
            _conexionEmpleado = new ConexionEmpleado();
        }

        // =================================================================
        // EMPLEADOS - Métodos para la Vista
        // =================================================================

        /// <summary>
        /// Obtiene la lista de empleados de la BD y la convierte a un formato DataTable 
        /// optimizado para DataGridView en WinForms.
        /// </summary>
        /// <returns>DataTable con los datos procesados, o DataTable vacío en caso de error o sin datos.</returns>
        public DataTable ObtenerEmpleadosParaVista()
        {
            // 1. Obtiene la lista de entidades (Empleado) desde la capa de datos (Modelo)
            List<Empleado> listaEmpleados = _conexionEmpleado.ObtenerTodosEmpleados();

            // 2. Manejo de errores y listas vacías
            if (listaEmpleados == null || !listaEmpleados.Any())
            {
                // En caso de error de conexión o lista vacía, devuelve una tabla vacía
                return new DataTable();
            }

            // 3. Define y estructura el DataTable (Formato para la Vista)
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Nombre Completo", typeof(string)); // Columna combinada
            dt.Columns.Add("Sueldo", typeof(decimal));
            dt.Columns.Add("Inicio Contrato", typeof(string)); // Fecha formateada
            dt.Columns.Add("Fin Contrato", typeof(string));    // Fecha formateada y manejo de NULL
            dt.Columns.Add("ID Rol", typeof(int));

            // 4. Mapeo y procesamiento de datos
            foreach (var empleado in listaEmpleados)
            {
                // Formato de fechas para mejor visualización
                string fechaInicio = empleado.FechaContratoInicio.ToShortDateString();

                // Manejo del valor nullable (DateTime?): si tiene valor, lo formatea; si es NULL, muestra "Activo".
                string fechaFin = empleado.FechaContratoFinal.HasValue
                                ? empleado.FechaContratoFinal.Value.ToShortDateString()
                                : "Activo"; // Un contrato sin fecha final está activo

                // Concatenación de nombre y apellidos para una sola columna
                string nombreCompleto = $"{empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}".Trim();

                dt.Rows.Add(
                    empleado.IdEmpleado,
                    nombreCompleto,
                    empleado.Sueldo,
                    fechaInicio,
                    fechaFin,
                    empleado.IdRol
                );
            }

            return dt;
        }

        // Aquí se agregarían otros métodos como:
        // public bool GuardarNuevoEmpleado(Empleado nuevoEmpleado) { ... }
        // public bool EliminarEmpleado(int id) { ... }
    }
}

