namespace FoodWare.Model.Entities
{
    /// <summary>
    /// DTO para mostrar la lista de empleados/usuarios en el DataGridView.
    /// Combina datos de Usuarios y Roles.
    /// </summary>
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty; // <-- El JOIN
        public bool Activo { get; set; }

        // Propiedad de solo lectura para la UI
        public string Estado => Activo ? "Activo" : "Inactivo";

        // Importante: Necesitamos guardar el IdRol para la edición
        public int IdRol { get; set; }
    }
}