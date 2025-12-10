using System.Collections.Generic;

namespace FoodWare.Shared.Entities
{
    /// <summary>
    /// Clase auxiliar para enviar una venta completa desde el móvil a la API
    /// </summary>
    public class VentaRequest
    {
        public int IdUsuario { get; set; }
        public string FormaDePago { get; set; } = "Efectivo";
        public List<DetalleVenta> Detalles { get; set; } = [];
    }
}