namespace FoodWare.Controller.Logic
{
    /// <summary>
    /// Almacena de forma estática y segura la información del usuario
    /// que ha iniciado sesión.
    /// Esta es la "fuente de verdad" para la autorización en toda la app.
    /// </summary>
    public static class UserSession
    {
        // Propiedades estáticas con 'set' privado.
        // Solo la propia clase puede establecer estos valores.
        public static int IdUsuario { get; private set; }
        public static string NombreCompleto { get; private set; } = string.Empty;
        public static string NombreRol { get; private set; } = string.Empty;

        /// <summary>
        /// Comprueba si hay un usuario válido en sesión.
        /// </summary>
        public static bool IsLoggedIn => IdUsuario > 0;

        /// <summary>
        /// El LoginController llama a este método DESPUÉS de 
        /// validar exitosamente las credenciales.
        /// </summary>
        public static void Login(int idUsuario, string nombreCompleto, string nombreRol)
        {
            if (idUsuario <= 0 || string.IsNullOrWhiteSpace(nombreRol))
            {
                throw new ArgumentException("No se puede iniciar sesión con datos de usuario no válidos.");
            }

            IdUsuario = idUsuario;
            NombreCompleto = nombreCompleto;
            NombreRol = nombreRol;
        }

        /// <summary>
        /// Limpia los datos de la sesión (ej. al cerrar sesión).
        /// </summary>
        public static void Logout()
        {
            IdUsuario = 0;
            NombreCompleto = string.Empty;
            NombreRol = string.Empty;
        }
    }
}