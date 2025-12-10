using System;

namespace FoodWare.Shared.Entities
{
    public class MovimientoInventario
    {
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string TipoMovimiento { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public string? Motivo { get; set; }
    }
}