using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 5. TABLA: MENU_ITEMS
    public class MenuItem
    {
        public int IdItem { get; set; }
        public string Nombre { get; set; } // NOT NULL
        public string Tipo { get; set; } // ENUM: Platillo, Bebida
        public string Descripcion { get; set; } // NULLable
        public decimal Precio { get; set; } // NOT NULL
        public string Moneda { get; set; } // NOT NULL, DEFAULT 'MXN'
        public decimal? GramajeOMl { get; set; } // NULLable (usamos decimal?)
    }
}
