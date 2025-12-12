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
        Task<List<UsuarioDto>> ObtenerTodosDtoAsync();
        Task AgregarAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task ActualizarPasswordAsync(int idUsuario, string passwordHash);
        Task EliminarAsync(int idUsuario);
    }
}