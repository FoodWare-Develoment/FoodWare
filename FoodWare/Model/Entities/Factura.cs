using System;
using System.Collections.Generic;

namespace FoodWare.Model.Entities
{
    // 9. TABLA: FACTURAS
    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdVenta { get; set; } // UNIQUE, NOT NULL
        public string NombreFactura { get; set; } // NOT NULL
        public string Rfc { get; set; } // NOT NULL
        public string FormaPagoSat { get; set; } // NOT NULL
        public string UsoCfdi { get; set; } // NOT NULL
        public decimal MontoFacturado { get; set; } // NOT NULL
        public string RegimenFiscal { get; set; } // NULLable
        public DateTime FechaEmision { get; set; } // NOT NULL, DEFAULT CURRENT_TIMESTAMP
    }
}
