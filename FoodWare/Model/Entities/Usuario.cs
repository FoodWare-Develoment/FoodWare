using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 3. TABLA: USUARIOS
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; } // NOT NULL
        public string PasswordHash { get; set; } // NOT NULL
        public int? IdEmpleado { get; set; } // UNIQUE, NULLable (usamos int?)
        public bool EsAdmin { get; set; } // NOT NULL, DEFAULT FALSE
    }
}
