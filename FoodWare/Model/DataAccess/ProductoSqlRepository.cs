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
            using var connection = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Productos (Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto) VALUES (@Nombre, @Categoria, @UnidadMedida, @StockActual, @StockMinimo, @PrecioCosto);";
            connection.Execute(sql, producto);
        }

        public void Eliminar(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Productos WHERE IdProducto = @Id;";
            connection.Execute(sql, new { Id = id });
        }

        public List<Producto> ObtenerTodos()
        {
            using var connection = new SqlConnection(_connectionString);
            return [.. connection.Query<Producto>("SELECT IdProducto, Nombre, Categoria, UnidadMedida, StockActual, StockMinimo, PrecioCosto FROM Productos;")];
        }

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