using Dapper;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.API.DataAccess
{
    public class RolSqlRepository(IConfiguration configuration) : IRolRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("FoodWareDB")!;

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT IdRol, NombreRol FROM Roles ORDER BY NombreRol;";
            var roles = await connection.QueryAsync<Rol>(sql);
            return [.. roles];
        }
    }
}