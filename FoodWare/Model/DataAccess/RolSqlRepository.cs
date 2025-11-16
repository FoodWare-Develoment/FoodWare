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
    public class RolSqlRepository : IRolRepository
    {
        private readonly string _connectionString;

        public RolSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT IdRol, NombreRol FROM Roles ORDER BY NombreRol;";
            var roles = await connection.QueryAsync<Rol>(sql);
            return [.. roles];
        }
    }
}