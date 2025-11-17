namespace FoodWare.Model.Entities
{
    /// <summary>
    /// DTO para el Reporte de Mermas. Muestra las pérdidas por producto y motivo.
    /// </summary>
    public class ReporteMermasDto
    {
        public string Producto { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public decimal CantidadPerdida { get; set; }
        public decimal CostoPerdida { get; set; }
    }
}