using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using FoodWare.Controller;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    /// <summary>
    /// Implementación de IPlatilloRepository que utiliza SQL Server y Dapper para el acceso a datos.
    /// </summary>
    public class PlatilloSqlRepository : IPlatilloRepository
    {
        private readonly string _connectionString;

        public PlatilloSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task AgregarAsync(Platillo platillo)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Platillos (Nombre, Categoria, PrecioVenta) VALUES (@Nombre, @Categoria, @PrecioVenta);";
            // Usamos ExecuteAsync
            await connection.ExecuteAsync(sql, platillo);
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Platillos WHERE IdPlatillo = @Id;";
            // Usamos ExecuteAsync
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<List<Platillo>> ObtenerTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT IdPlatillo, Nombre, Categoria, PrecioVenta FROM Platillos;";
            // Usamos QueryAsync
            var platillos = await connection.QueryAsync<Platillo>(sql);
            return [.. platillos];
        }
                
        /// <summary>
        /// Actualiza un platillo existente en la base de datos.
        /// </summary>
        public async Task ActualizarAsync(Platillo platillo)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"UPDATE Platillos 
                           SET Nombre = @Nombre, 
                               Categoria = @Categoria, 
                               PrecioVenta = @PrecioVenta 
                           WHERE IdPlatillo = @IdPlatillo;";

            // Usamos ExecuteAsync para la operación de actualización
            await connection.ExecuteAsync(sql, platillo);
        }

        // --- Métodos Pendientes de Implementación ---
        public Task<Platillo> ObtenerPorIdAsync(int id)
            => throw new NotImplementedException("La funcionalidad de obtener por ID aún no está implementada.");
    }
}