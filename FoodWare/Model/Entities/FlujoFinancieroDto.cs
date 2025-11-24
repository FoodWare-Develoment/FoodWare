using System;

namespace FoodWare.Model.Entities
{
    /// <summary>
    /// Para el Gráfico de Barras: Muestra Ingreso vs Egreso por día.
    /// </summary>
    public class FlujoFinancieroDto
    {
        public DateTime Fecha { get; set; }
        public decimal IngresoTotal { get; set; }
        public decimal EgresoTotal { get; set; } // Suma de Costos + Mermas + Gastos
    }
}