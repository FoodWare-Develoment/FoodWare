using Dapper;
using FoodWare.Controller;
using FoodWare.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    /// <summary>
    /// Implementación SQL de IUsuarioRepository que utiliza Dapper.
    /// </summary>
    public class UsuarioSqlRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioSqlRepository()
        {
            // Obtenemos la cadena de conexión de la configuración global
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        /// <summary>
        /// Obtiene el hash de la contraseña de un usuario por su nombre de usuario.
        /// </summary>
        public async Task<string?> ObtenerHashPorUsuarioAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);

            // Esta es la consulta que tenías en tu LoginController
            string query = "SELECT ContraseñaHash FROM Usuarios WHERE NombreUsuario = @username AND Activo = 1";

            // Usamos QuerySingleOrDefaultAsync de Dapper para obtener un solo resultado (o null)
            var storedPasswordHash = await connection.QuerySingleOrDefaultAsync<string>(query, new { username });

            return storedPasswordHash;
        }
    }
}