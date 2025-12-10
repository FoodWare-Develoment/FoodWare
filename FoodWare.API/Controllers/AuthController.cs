using Microsoft.AspNetCore.Mvc;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using BCrypt.Net;

namespace FoodWare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUsuarioRepository usuarioRepo) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            // 1. Buscar al usuario en la BD
            var usuarioInfo = await usuarioRepo.ObtenerLoginInfoPorUsuarioAsync(request.Usuario);

            // 2. Si no existe
            if (usuarioInfo == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            // 3. Verificar la contraseña
            bool passValida = BCrypt.Net.BCrypt.Verify(request.Password, usuarioInfo.ContraseñaHash);

            if (!passValida)
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // 4. Devolvemos los datos del usuario
            return Ok(new LoginResponse
            {
                IdUsuario = usuarioInfo.IdUsuario,
                NombreCompleto = usuarioInfo.NombreCompleto,
                Rol = usuarioInfo.NombreRol
            });
        }
    }
}