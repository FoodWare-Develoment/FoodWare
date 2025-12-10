using Dapper;
using FoodWare.Controller;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    public class RecetaSqlRepository : IRecetaRepository
    {
        private readonly string _connectionString;

        public RecetaSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task AgregarAsync(Receta receta)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"INSERT INTO Recetas (IdPlatillo, IdProducto, Cantidad) 
                           VALUES (@IdPlatillo, @IdProducto, @Cantidad);";
            await connection.ExecuteAsync(sql, receta);
        }

        public async Task EliminarAsync(int idReceta)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Recetas WHERE IdReceta = @IdReceta;";
            await connection.ExecuteAsync(sql, new { IdReceta = idReceta });
        }

        public async Task<List<RecetaDetalle>> ObtenerPorPlatilloAsync(int idPlatillo)
        {
            using var connection = new SqlConnection(_connectionString);
            // Query con JOIN para obtener los datos de la DTO
            string sql = @"SELECT 
                               R.IdReceta, 
                               R.IdProducto, 
                               P.Nombre AS NombreProducto, 
                               R.Cantidad, 
                               P.UnidadMedida,
                               P.PrecioCosto -- <-- AÑADIDO PARA C-6 (Costo Receta)
                           FROM 
                               Recetas R 
                           INNER JOIN 
                               Productos P ON R.IdProducto = P.IdProducto
                           WHERE 
                               R.IdPlatillo = @IdPlatillo;";

            var detalles = await connection.QueryAsync<RecetaDetalle>(sql, new { IdPlatillo = idPlatillo });
            return [.. detalles];
        }
        public async Task<List<RecetaDetalle>> ObtenerPorPlatilloAsync(int idPlatillo, SqlConnection connection, SqlTransaction transaction)
        {
            // La consulta es la misma, solo cambian los parámetros
            string sql = @"SELECT 
                       R.IdReceta, 
                       R.IdProducto, 
                       P.Nombre AS NombreProducto, 
                       R.Cantidad, 
                       P.UnidadMedida,
                       P.PrecioCosto -- <-- AÑADIDO PARA C-6 (Costo Receta)
                   FROM 
                       Recetas R 
                   INNER JOIN 
                       Productos P ON R.IdProducto = P.IdProducto
                   WHERE 
                       R.IdPlatillo = @IdPlatillo;";

            var detalles = await connection.QueryAsync<RecetaDetalle>(sql,
                new { IdPlatillo = idPlatillo },
                transaction);

            return [.. detalles];
        }

        public async Task<bool> ProductoEstaEnUsoAsync(int idProducto)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT 1 FROM Recetas WHERE IdProducto = @IdProducto;";
            // QueryFirstOrDefaultAsync devuelve 0 (default) o 1
            var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new { IdProducto = idProducto });
            return result > 0;
        }
    }
}