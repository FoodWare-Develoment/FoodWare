using BCrypt.Net;
using System;
using System.Diagnostics;
using FoodWare.Model.Interfaces;
using System.Threading.Tasks;
using FoodWare.Model.DataAccess;

namespace FoodWare.Controller.Logic
{
    public class LoginController(IUsuarioRepository usuarioRepository)
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        /// <summary>
        /// Valida las credenciales y devuelve el resultado y el Rol del usuario.
        /// </summary>
        /// <returns>Un Task que representa una tupla (LoginResult, string? Rol)</returns>
        public async Task<(LoginResult Resultado, string? Rol)> ValidarLoginAsync(string username, string password)
        {
            try
            {
                // 1. Obtenemos el hash
                var storedPasswordHash = await _usuarioRepository.ObtenerHashPorUsuarioAsync(username);

                // 2. Si no hay hash, el usuario no existe o está inactivo
                if (string.IsNullOrEmpty(storedPasswordHash))
                {
                    return (LoginResult.CredencialesInvalidas, null);
                }

                // 3. Verificamos el hash
                storedPasswordHash = storedPasswordHash.Trim();
                bool esValido = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);

                if (esValido)
                {
                    // 4. Si es válido, obtenemos su Rol
                    string? rol = await _usuarioRepository.ObtenerRolPorNombreUsuarioAsync(username);
                    return (LoginResult.Exitoso, rol);
                }
                else
                {
                    return (LoginResult.CredencialesInvalidas, null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error de base de datos al validar login: {ex.Message}");
                return (LoginResult.ErrorDeBaseDeDatos, null);
            }
        }
    }
}