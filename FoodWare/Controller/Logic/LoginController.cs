using BCrypt.Net;
using System;
using System.Diagnostics;
using FoodWare.Model.Interfaces; 
using System.Threading.Tasks;
using FoodWare.Model.DataAccess;

namespace FoodWare.Controller.Logic
{
    /// <summary>
    /// Gestiona la lógica de negocio para la autenticación de usuarios.
    /// Ahora usa un Repositorio para el acceso a datos.
    /// </summary>
    /// <remarks>
    /// Constructor: Se ejecuta una vez cuando se crea el LoginController.
    /// Recibe el repositorio de usuarios (Inyección de Dependencias).
    /// </remarks>
    public class LoginController(IUsuarioRepository usuarioRepository)
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        /// <summary>
        /// Valida las credenciales de un usuario contra la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="username">El nombre de usuario ingresado en el formulario.</param>
        /// <param name="password">La contraseña en texto plano ingresada en el formulario.</param>
        /// <returns>Un Task que representa un valor del enum LoginResult.</returns>
        public async Task<LoginResult> ValidarLoginAsync(string username, string password)
        {
            try
            {
                // 1. Obtenemos el hash desde el repositorio de forma asíncrona
                var storedPasswordHash = await _usuarioRepository.ObtenerHashPorUsuarioAsync(username);

                // 2. Si la consulta no devolvió nada, el usuario no existe o no está activo.
                if (string.IsNullOrEmpty(storedPasswordHash))
                {
                    return LoginResult.CredencialesInvalidas;
                }

                // 3. (Lógica de Negocio) Verificamos el hash
                storedPasswordHash = storedPasswordHash.Trim();
                bool esValido = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);

                return esValido ? LoginResult.Exitoso : LoginResult.CredencialesInvalidas;
            }
            catch (Exception ex)
            {
                // Si cualquier cosa falla (ej. BD offline), lo capturamos.
                Debug.WriteLine($"Error de base de datos al validar login: {ex.Message}");
                return LoginResult.ErrorDeBaseDeDatos;
            }
        }
    }
}