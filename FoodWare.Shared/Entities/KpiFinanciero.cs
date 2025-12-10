namespace FoodWare.Shared.Entities
{
    /// <summary>
    /// DTO Agregado para el Dashboard de Finanzas.
    /// Contiene los 4 números críticos para la toma de decisiones.
    /// </summary>
    public class KpiFinanciero
    {
        /// <summary>
        /// Tarjeta 1 (Verde): Dinero total entrante por ventas.
        /// </summary>
        public decimal IngresosBrutos { get; set; }

        /// <summary>
        /// Tarjeta 2 (Naranja): Costo de ingredientes vendidos + Costo de Mermas.
        /// Representa el costo directo de la comida.
        /// </summary>
        public decimal CostoYPerdida { get; set; }

        /// <summary>
        /// Tarjeta 3 (Rojo): Suma de gastos operativos (Luz, Renta, etc.).
        /// </summary>
        public decimal GastosOperativos { get; set; }

        /// <summary>
        /// Tarjeta 4 (Azul): La verdad del negocio.
        /// Fórmula: Ingresos - (CostoYPerdida + GastosOperativos).
        /// </summary>
        public decimal UtilidadNetaReal { get; set; }
    }
}