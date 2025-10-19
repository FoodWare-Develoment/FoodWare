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
    /// <summary>
    /// Implementación de IProductoRepository que utiliza SQL Server y Dapper para el acceso a datos.
    /// </summary>
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
            // Usamos QueryAsync
            var productos = await connection.QueryAsync<Producto>("SELECT IdProducto, Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto FROM Productos;");
            return [.. productos];
        }

        public async Task AgregarAsync(Producto producto)
        {
            using var connection = new SqlConnection(_connectionString);
            // Esta es tu consulta SQL original, que es la correcta.
            string sql = "INSERT INTO Productos (Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto) VALUES (@Nombre, @Categoria, @UnidadMedida, @StockActual, @StockMinimo, @PrecioCosto);";
            // Usamos ExecuteAsync
            await connection.ExecuteAsync(sql, producto);
        }

        public async Task EliminarAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Productos WHERE IdProducto = @Id;";

            // Usamos ExecuteAsync en lugar de Execute
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        // --- Métodos Pendientes de Implementación ---

        public Task ActualizarAsync(Producto producto) 
            => throw new NotImplementedException("La funcionalidad de actualizar aún no está implementada.");


        public Task<Producto> ObtenerPorIdAsync(int id) 
            => throw new NotImplementedException("La funcionalidad de obtener por ID aún no está implementada.");
        
        
    }
}