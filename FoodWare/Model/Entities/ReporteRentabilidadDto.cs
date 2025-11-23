namespace FoodWare.Model.Entities
{
    /// <summary>
    /// DTO para el Reporte de Rentabilidad.
    /// Muestra la ganancia bruta por platillo.
    /// </summary>
    public class ReporteRentabilidadDto
    {
        public string Platillo { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public decimal CostoReceta { get; set; }
        public decimal GananciaBruta { get; set; }
        public int UnidadesVendidas { get; set; }
        public decimal GananciaTotal { get; set; }
    }
}