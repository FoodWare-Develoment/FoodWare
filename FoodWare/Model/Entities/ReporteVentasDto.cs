using System;

namespace FoodWare.Model.Entities
{
    /// <summary>
    /// DTO para el Reporte de Ventas
    /// </summary>
    public class ReporteVentasDto
    {
        public DateTime Dia { get; set; }
        public decimal TotalDiario { get; set; }
    }
}