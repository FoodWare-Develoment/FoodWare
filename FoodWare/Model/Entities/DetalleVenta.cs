namespace FoodWare.Model.Entities
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdPlatillo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; } // El precio al que se vendió

        // Propiedad de navegación (opcional, pero útil para la UI)
        // No está en la BD, pero la llenaremos desde el código.
        public string NombrePlatillo { get; set; } = string.Empty;
    }
}