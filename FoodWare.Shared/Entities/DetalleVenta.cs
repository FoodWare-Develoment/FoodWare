namespace FoodWare.Shared.Entities
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdPlatillo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; } // El precio al que se vendió
        public string NombrePlatillo { get; set; } = string.Empty;
    }
}