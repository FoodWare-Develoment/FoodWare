using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FoodWare.Controller;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FoodWare.Model.DataAccess
{
    public class ProductoSqlRepository : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var productos = await connection.QueryAsync<Producto>("SELECT IdProducto, Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto FROM Productos;");
            return [.. productos];
        }

        public async Task AgregarAsync(Producto producto)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Productos (Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto) VALUES (@Nombre, @Categoria, @UnidadMedida, @StockActual, @StockMinimo, @PrecioCosto);";
            await connection.ExecuteAsync(sql, producto);
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Productos WHERE IdProducto = @Id;";
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task ActualizarAsync(Producto producto)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"UPDATE Productos 
                           SET Nombre = @Nombre, 
                               Categoria = @Categoria, 
                               UnidadMedida = @UnidadMedida, 
                               StockActual = @StockActual, 
                               StockMinimo = @StockMinimo, 
                               PrecioCosto = @PrecioCosto 
                           WHERE IdProducto = @IdProducto;";
            await connection.ExecuteAsync(sql, producto);
        }

        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Productos WHERE IdProducto = @Id;";
            // Usamos QuerySingleOrDefaultAsync para obtener un solo producto o null
            return await connection.QuerySingleOrDefaultAsync<Producto>(sql, new { Id = id });
        }

        public async Task<Dictionary<int, decimal>> ObtenerMapaStockAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT IdProducto, StockActual FROM Productos;";
            var stocks = await connection.QueryAsync<(int IdProducto, decimal StockActual)>(sql);
            return stocks.ToDictionary(item => item.IdProducto, item => item.StockActual);
        }
    }
}