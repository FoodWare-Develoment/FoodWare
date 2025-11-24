namespace FoodWare.Model.Entities
{
    /// <summary>
    /// Para el Gráfico de Pastel: Categoría de gasto y monto total.
    /// </summary>
    public class DesgloseGastosDto
    {
        public string Categoria { get; set; } = string.Empty; // Ej: "Mermas", "Insumos", "Nómina"
        public decimal MontoTotal { get; set; }
    }
}