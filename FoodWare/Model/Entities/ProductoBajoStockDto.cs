namespace FoodWare.Model.Entities
{
    public class ProductoBajoStockDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public decimal CantidadAReordenar { get; set; }
        public long Prioridad { get; set; } 
    }
}