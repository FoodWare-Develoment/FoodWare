using BCrypt.Net;
using System;
using System.Diagnostics;
using FoodWare.Shared.Interfaces;
using System.Threading.Tasks;
using FoodWare.Model.DataAccess;
using FoodWare.Shared.Entities;

namespace FoodWare.Controller.Logic
{
    public class LoginController(IUsuarioRepository usuarioRepository)
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        /// <summary>
        /// Valida las credenciales y devuelve el resultado y el Rol del usuario.
        /// </summary>
        /// <returns>Un Task que representa una tupla (LoginResult, string? Rol)</returns>
        public async Task<LoginResult> ValidarLoginAsync(string username, string password)
        {
            try
            {
                // 1. Obtenemos el DTO de Login
                var loginInfo = await _usuarioRepository.ObtenerLoginInfoPorUsuarioAsync(username);

                // 2. Si no hay DTO, el usuario no existe o está inactivo
                if (loginInfo == null || string.IsNullOrEmpty(loginInfo.ContraseñaHash))
                {
                    return LoginResult.CredencialesInvalidas;
                }

                // 3. Verificamos el hash
                bool esValido = BCrypt.Net.BCrypt.Verify(password, loginInfo.ContraseñaHash.Trim());

                if (esValido)
                {
                    // 4. Si es válido, ESTABLECEMOS LA SESIÓN
                    UserSession.Login(
                        loginInfo.IdUsuario,
                        loginInfo.NombreCompleto,
                        loginInfo.NombreRol
                    );

                    return LoginResult.Exitoso;
                }
                else
                {
                    return LoginResult.CredencialesInvalidas;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error de base de datos al validar login: {ex.Message}");
                return LoginResult.ErrorDeBaseDeDatos;
            }
        }
    }
}