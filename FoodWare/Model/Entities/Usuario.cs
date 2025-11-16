using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Model.Entities
{
    /// <summary>
    /// Representa la tabla [dbo].[Usuarios] en la base de datos.
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string NombreCompleto { get; set; }
        public required string NombreUsuario { get; set; }
        public required string ContraseñaHash { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }
    }
}