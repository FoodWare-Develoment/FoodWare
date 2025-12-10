using System;

namespace FoodWare.Shared.Entities
{
    /// <summary>
    /// Registro de auditoría de un corte de caja.
    /// Almacena la diferencia entre lo que el sistema esperaba y lo que el cajero contó.
    /// </summary>
    public class CorteCaja
    {
        public int IdCorte { get; set; }
        public DateTime FechaCierre { get; set; } = DateTime.Now;
        public int IdUsuarioCajero { get; set; }

        public decimal FondoInicial { get; set; }
        public decimal TotalEfectivoEsperado { get; set; } // Lo que dice el sistema (Ventas Efec)
        public decimal TotalEfectivoContado { get; set; }  // Lo que el cajero ingresó a ciegas

        public decimal Diferencia { get; set; } // (Contado - Esperado)
        public string SobranteFaltante { get; set; } = string.Empty; // "Sobrante", "Faltante", "Cuadrado"
    }
}