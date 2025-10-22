using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 1. TABLA: ROL (id_rol, categoria)
    public class Rol
    {
        public int IdRol { get; set; }
        public string Categoria { get; set; } // NOT NULL
    }
}
