using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 7. TABLA: VENTAS
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaCobro { get; set; } // NOT NULL, DEFAULT CURRENT_TIMESTAMP
        public string FormaPago { get; set; } // NOT NULL
        public decimal MontoTotal { get; set; } // NOT NULL
        public string Moneda { get; set; } // NOT NULL, DEFAULT 'MXN'
        public int? IdMesero { get; set; } // FOREIGN KEY, NULLable (usamos int?)
        public int? IdChef { get; set; } // FOREIGN KEY, NULLable (usamos int?)
    }
}
