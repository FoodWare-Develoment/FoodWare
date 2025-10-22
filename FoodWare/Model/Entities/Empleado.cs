using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Este archivo representa la estructura de los datos (la Entidad)
// No contiene lógica de base de datos, solo propiedades.

namespace FoodWare.Model.Entities
{
    public class Empleado
    {
        // Propiedades mapeadas a las columnas de la tabla EMPLEADOS
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public decimal Sueldo { get; set; }
        public DateTime FechaContratoInicio { get; set; }
        public DateTime? FechaContratoFinal { get; set; } // Nullable, por si el contrato sigue activo
        public int IdRol { get; set; }
    }
}
