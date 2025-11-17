using System;

namespace FoodWare.Controller.Exceptionss
{
    /// <summary>
    /// Excepción personalizada que se lanza cuando la validación de stock falla.
    /// Contiene información detallada sobre el fallo.
    /// </summary>
    public class StockInsuficienteException(string mensaje, string platillo, string ingrediente, int maxVendible) : Exception(mensaje)
    {
        public string PlatilloConProblema { get; } = platillo;
        public string IngredienteFaltante { get; } = ingrediente;
        public int MaximaCantidadVendible { get; } = maxVendible;
    }
}