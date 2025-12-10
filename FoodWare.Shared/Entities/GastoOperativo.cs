using System;

namespace FoodWare.Shared.Entities
{
    /// <summary>
    /// Representa un gasto operativo manual (Luz, Renta, Nómina, etc.)
    /// que afecta la utilidad neta pero no el inventario de ingredientes.
    /// </summary>
    public class GastoOperativo
    {
        public int IdGasto { get; set; }
        public required string Concepto { get; set; } // Ej: "Recibo CFE Noviembre"
        public decimal Monto { get; set; }
        public required string Categoria { get; set; } // Ej: "Servicios", "Nómina"
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int IdUsuario { get; set; } // Quién registró el gasto
    }
}