using FoodWare.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Shared.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<string?> ObtenerHashPorUsuarioAsync(string username);
        Task<string?> ObtenerRolPorNombreUsuarioAsync(string username);
        Task<LoginInfo?> ObtenerLoginInfoPorUsuarioAsync(string username);

        // --- MÉTODOS CRUD ---
        Task<List<UsuarioDto>> ObtenerTodosDtoAsync();
        Task AgregarAsync(Usuario usuario);

        /// <summary>
        /// Actualiza los datos de un usuario existente (excepto contraseña).
        /// </summary>
        Task ActualizarAsync(Usuario usuario);

        /// <summary>
        /// Actualiza únicamente la contraseña hasheada de un usuario.
        /// </summary>
        Task ActualizarPasswordAsync(int idUsuario, string passwordHash);
    }
}