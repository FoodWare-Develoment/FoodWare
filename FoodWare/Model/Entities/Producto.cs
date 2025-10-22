using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 4. TABLA: PRODUCTOS
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } // NOT NULL
        public string UnidadMedida { get; set; } // NOT NULL
        public decimal StockActual { get; set; } // NOT NULL
        public decimal? PrecioCompra { get; set; } // NULLable (usamos decimal?)
        public DateTime? FechaCompra { get; set; } // NULLable (usamos DateTime?)
        public DateTime? FechaCaducidad { get; set; } // NULLable (usamos DateTime?)
    }
}
