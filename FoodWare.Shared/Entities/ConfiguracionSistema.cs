namespace FoodWare.Shared.Entities
{
    /// <summary>
    /// Representa la configuración general del establecimiento.
    /// Solo debe existir un registro de este tipo en la BD.
    /// </summary>
    public class ConfiguracionSistema
    {
        public int IdConfig { get; set; }
        public required string NombreRestaurante { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public decimal PorcentajeImpuesto { get; set; }
        public string Moneda { get; set; } = "MXN";
        public string MensajeTicket { get; set; } = string.Empty;
    }
}