using FoodWare.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    /// <summary>
    /// Define las operaciones de datos para la entidad Usuario.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Obtiene el hash de la contraseña de un usuario por su nombre de usuario.
        /// </summary>
        Task<string?> ObtenerHashPorUsuarioAsync(string username);

        /// <summary>
        /// Obtiene el nombre del Rol de un usuario.
        /// </summary>
        Task<string?> ObtenerRolPorNombreUsuarioAsync(string username);

        // --- MÉTODOS CRUD ---

        /// <summary>
        /// Obtiene la lista de todos los usuarios con su rol (DTO).
        /// </summary>
        Task<List<UsuarioDto>> ObtenerTodosDtoAsync();

        /// <summary>
        /// Agrega un nuevo usuario a la base de datos.
        /// </summary>
        Task AgregarAsync(Usuario usuario);

        /// <summary>
        /// Actualiza los datos de un usuario existente (excepto contraseña).
        /// </summary>
        Task ActualizarAsync(Usuario usuario);

        /// <summary>
        /// Desactiva (baja lógica) un usuario en la base de datos.
        /// </summary>
        Task DesactivarAsync(int idUsuario);
    }
}