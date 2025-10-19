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
        /// <param name="username">El nombre de usuario a buscar.</param>
        /// <returns>Una tarea que representa la operación, con el hash de la contraseña o null si no se encuentra.</returns>
        Task<string?> ObtenerHashPorUsuarioAsync(string username);
    }
}