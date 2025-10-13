using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Agregar(Producto producto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                string sql = "INSERT INTO Productos (Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto) VALUES (@Nombre, @Categoria, @UnidadMedida, @StockActual, @StockMinimo, @PrecioCosto);";
                connection.Execute(sql, producto);
            }
            catch (SqlException ex)
            {
                // Capturamos errores específicos de SQL para manejarlos de forma controlada.
                // A futuro, esta sección es ideal para registrar el error en un log.
                Exception exception = new($"Error en la base de datos al intentar agregar un producto: {ex.Message}", ex);
                throw exception;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                string sql = "DELETE FROM Productos WHERE IdProducto = @Id;";
                connection.Execute(sql, new { Id = id });
            }
            catch (SqlException ex)
            {
                Exception exception = new($"Error en la base de datos al intentar eliminar el producto con ID {id}: {ex.Message}", ex);
                throw exception;
            }
        }

        public List<Producto> ObtenerTodos()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return [.. connection.Query<Producto>("SELECT IdProducto, Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto FROM Productos;")];
            }
            catch (SqlException ex)
            {
                Exception exception = new($"Error en la base de datos al obtener la lista de productos: {ex.Message}", ex);
                throw exception;
            }
        }

        // --- Métodos Pendientes de Implementación ---

        public void Actualizar(Producto producto)
        {
            throw new NotImplementedException("La funcionalidad de actualizar aún no está implementada.");
        }

        public Producto ObtenerPorId(int id)
        {
            throw new NotImplementedException("La funcionalidad de obtener por ID aún no está implementada.");
        }
    }
}