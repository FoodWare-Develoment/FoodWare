using System;

namespace FoodWare.Shared.Exceptions // <-- Namespace correcto
{
    public class StockInsuficienteException(string mensaje, string platillo, string ingrediente, int maxVendible) : Exception(mensaje)
    {
        public string PlatilloConProblema { get; } = platillo;
        public string IngredienteFaltante { get; } = ingrediente;
        public int MaximaCantidadVendible { get; } = maxVendible;
    }
}