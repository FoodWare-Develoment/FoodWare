using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using System; // Necesario para Exception
using System.Diagnostics; // Necesario para Debug.WriteLine

namespace FoodWare.Controller.Logic
{
    /// <summary>
    /// MEJORA 3: Documentación restaurada.
    /// Gestiona la lógica de negocio para la autenticación de usuarios.
    /// </summary>
    public class LoginController
    {
        private readonly string connectionString;

        public LoginController()
        {
            connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        /// <summary>
        /// MEJORA 3: Documentación restaurada.
        /// Valida las credenciales de un usuario contra la base de datos de forma segura.
        /// </summary>
        /// <param name="username">El nombre de usuario ingresado en el formulario.</param>
        /// <param name="password">La contraseña en texto plano ingresada en el formulario.</param>
        /// <returns>Devuelve 'true' si las credenciales son válidas y el usuario está activo. De lo contrario, devuelve 'false'.</returns>
        public bool ValidarLogin(string username, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ContraseñaHash FROM Usuarios WHERE NombreUsuario = @username AND Activo = 1";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        var storedPasswordHash = command.ExecuteScalar() as string;

                        if (string.IsNullOrEmpty(storedPasswordHash))
                        {
                            return false; // Usuario no encontrado o inactivo
                        }

                        storedPasswordHash = storedPasswordHash.Trim();

                        // MEJORA 2: Se corrige la verificación para que use la variable 'password'
                        bool verificacionDeBD = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);

                        return verificacionDeBD;
                    }
                }
                catch (Exception ex)
                {
                    // MEJORA 1: Se registra el detalle de la excepción en la ventana de Salida de Visual Studio.
                    // Esto es invaluable para diagnosticar problemas sin mostrar errores técnicos al usuario.
                    Debug.WriteLine($"Error al validar login para el usuario '{username}': {ex.Message}");
                    return false; // Devuelve false por seguridad ante cualquier error de BD.
                }
            }
        }
    }
}