using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 6. TABLA: RECETAS
    public class Receta
    {
        public int IdReceta { get; set; }
        public int IdItem { get; set; } // FOREIGN KEY, NOT NULL
        public int IdProducto { get; set; } // FOREIGN KEY, NOT NULL
        public decimal CantidadRequerida { get; set; } // NOT NULL
    }
}
