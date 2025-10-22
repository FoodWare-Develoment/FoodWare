using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 8. TABLA: DETALLE_VENTA
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }
        public int IdVenta { get; set; } // FOREIGN KEY, NOT NULL
        public int IdItem { get; set; } // FOREIGN KEY, NOT NULL
        public int Cantidad { get; set; } // NOT NULL, DEFAULT 1
        public decimal PrecioUnitarioVenta { get; set; } // NOT NULL
    }
}
