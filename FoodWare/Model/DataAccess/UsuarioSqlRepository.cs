using Dapper;
using FoodWare.Controller;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task<string?> ObtenerHashPorUsuarioAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "SELECT ContraseñaHash FROM Usuarios WHERE NombreUsuario = @username AND Activo = 1";
            var storedPasswordHash = await connection.QuerySingleOrDefaultAsync<string>(query, new { username });
            return storedPasswordHash;
        }

        public async Task<string?> ObtenerRolPorNombreUsuarioAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            // Consulta con JOIN para obtener el NombreRol
            string sql = @"
                SELECT 
                    R.NombreRol 
                FROM 
                    Usuarios U
                INNER JOIN 
                    Roles R ON U.IdRol = R.IdRol
                WHERE 
                    U.NombreUsuario = @username AND U.Activo = 1;";

            return await connection.QuerySingleOrDefaultAsync<string>(sql, new { username });
        }


        // --- Métodos CRUD ---
        public async Task<List<UsuarioDto>> ObtenerTodosDtoAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                SELECT 
                    U.IdUsuario, 
                    U.NombreCompleto, 
                    U.NombreUsuario, 
                    U.Activo,
                    U.IdRol,
                    R.NombreRol
                FROM 
                    Usuarios U
                INNER JOIN 
                    Roles R ON U.IdRol = R.IdRol
                ORDER BY
                    U.NombreCompleto;";

            var usuarios = await connection.QueryAsync<UsuarioDto>(sql);
            return [.. usuarios];
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                INSERT INTO Usuarios (NombreCompleto, NombreUsuario, ContraseñaHash, IdRol, Activo)
                VALUES (@NombreCompleto, @NombreUsuario, @ContraseñaHash, @IdRol, @Activo);";

            await connection.ExecuteAsync(sql, usuario);
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                UPDATE Usuarios SET
                    NombreCompleto = @NombreCompleto,
                    NombreUsuario = @NombreUsuario,
                    IdRol = @IdRol,
                    Activo = @Activo
                WHERE
                    IdUsuario = @IdUsuario;";

            await connection.ExecuteAsync(sql, usuario);
        }

        public async Task DesactivarAsync(int idUsuario)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "UPDATE Usuarios SET Activo = 0 WHERE IdUsuario = @IdUsuario;";
            await connection.ExecuteAsync(sql, new { IdUsuario = idUsuario });
        }
    }
}