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
    /// Implementación de IPlatilloRepository que utiliza SQL Server y Dapper para el acceso a datos.
    /// </summary>
    public class PlatilloSqlRepository : IPlatilloRepository
    {
        private readonly string _connectionString;

        public PlatilloSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public void Agregar(Platillo platillo)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                string sql = "INSERT INTO Platillos (Nombre, Categoria, PrecioVenta) VALUES (@Nombre, @Categoria, @PrecioVenta);";
                connection.Execute(sql, platillo);
            }
            catch (SqlException ex)
            {
                // Capturamos errores específicos de SQL para manejarlos de forma controlada.
                // A futuro, esta sección es ideal para registrar el error en un log.
                Exception exception = new($"Error en la base de datos al intentar agregar un platillo: {ex.Message}", ex);
                throw exception;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                string sql = "DELETE FROM Platillos WHERE IdPlatillo = @Id;";
                connection.Execute(sql, new { Id = id });
            }
            catch (SqlException ex)
            {
                Exception exception = new($"Error en la base de datos al intentar eliminar el platillo con ID {id}: {ex.Message}", ex);
                throw exception;
            }
        }

        public List<Platillo> ObtenerTodos()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                string sql = "SELECT IdPlatillo, Nombre, Categoria, PrecioVenta FROM Platillos;";
                return [.. connection.Query<Platillo>(sql)];
            }
            catch (SqlException ex)
            {
                Exception exception = new($"Error en la base de datos al obtener la lista de platillos: {ex.Message}", ex);
                throw exception;
            }
        }

        // --- Métodos Pendientes de Implementación ---

        public void Actualizar(Platillo platillo)
        {
            throw new NotImplementedException("La funcionalidad de actualizar platillo aún no está implementada.");
        }

        public Platillo ObtenerPorId(int id)
        {
            throw new NotImplementedException("La funcionalidad de obtener platillo por ID aún no está implementada.");
        }
    }
}