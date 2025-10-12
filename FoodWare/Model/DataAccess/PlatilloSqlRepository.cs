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
            // Tomamos la cadena de conexión configurada en Program.cs
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        /// <summary>
        /// Agrega un nuevo platillo a la base de datos.
        /// </summary>
        public void Agregar(Platillo platillo)
        {
            using var connection = new SqlConnection(_connectionString);

            // La columna 'Activo' se gestiona con un valor por defecto en la BD, por lo que no es necesaria aquí.
            string sql = "INSERT INTO Platillos (Nombre, Categoria, PrecioVenta) VALUES (@Nombre, @Categoria, @PrecioVenta);";
            connection.Execute(sql, platillo);
        }

        /// <summary>
        /// Elimina un platillo de la base de datos por su ID.
        /// </summary>
        public void Eliminar(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Platillos WHERE IdPlatillo = @Id;";
            connection.Execute(sql, new { Id = id });
        }

        /// <summary>
        /// Obtiene todos los platillos activos de la base de datos.
        /// </summary>
        public List<Platillo> ObtenerTodos()
        {
            using var connection = new SqlConnection(_connectionString);
            // Quitamos el filtro "WHERE Activo = 1"
            string sql = "SELECT IdPlatillo, Nombre, Categoria, PrecioVenta FROM Platillos;";
            return [.. connection.Query<Platillo>(sql)];
        }

        // --- Métodos Pendientes de Implementación ---
        // Al igual que en el repositorio de productos, puedes implementarlos cuando los necesites.

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