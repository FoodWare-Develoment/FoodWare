namespace FoodWare.Model.Entities
{
    /// <summary>
    /// DTO que contiene la información esencial del usuario
    /// recuperada durante el inicio de sesión.
    /// </summary>
    public class LoginInfo
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        public string ContraseñaHash { get; set; } = string.Empty;
    }
}