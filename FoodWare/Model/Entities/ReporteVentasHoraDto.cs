namespace FoodWare.Model.Entities
{
    /// <summary>
    /// DTO para el Reporte de Ventas por Hora.
    /// Muestra el flujo de ventas a lo largo del día.
    /// </summary>
    public class ReporteVentasHoraDto
    {
        public int Hora { get; set; }
        public decimal TotalVendido { get; set; }
        public int NumeroVentas { get; set; }
    }
}