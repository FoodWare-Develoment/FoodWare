using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Model.Entities
{
    /// <summary>
    /// Representa un usuario del sistema (Empleado o Administrador).
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Contraseña { get; set; } // En el futuro será un hash
        public required string Rol { get; set; } // Ej. "Administrador", "Mesero"
    }
}
