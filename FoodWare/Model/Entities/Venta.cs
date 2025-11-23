using System;

namespace FoodWare.Model.Entities
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        public int IdUsuario { get; set; } // Por ahora lo pondremos fijo, luego vendrá del Login
        public string FormaDePago { get; set; } = "Efectivo";
        public decimal TotalVenta { get; set; }
    }
}