using Dapper;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FoodWare.API.DataAccess
{
    public class VentaSqlRepository : IVentaRepository
    {
        // Este método recibe la conexión y transacción del Controlador
        public async Task<int> AgregarVentaAsync(Venta venta, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"INSERT INTO Ventas (FechaVenta, IdUsuario, FormaDePago, TotalVenta) 
                           VALUES (@FechaVenta, @IdUsuario, @FormaDePago, @TotalVenta);
                           SELECT SCOPE_IDENTITY();";

            // Usamos ExecuteScalarAsync para obtener el ID generado
            return await connection.ExecuteScalarAsync<int>(sql, venta, transaction);
        }

        public async Task AgregarDetalleAsync(DetalleVenta detalle, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"INSERT INTO DetallesVenta (IdVenta, IdPlatillo, Cantidad, PrecioUnitario) 
                           VALUES (@IdVenta, @IdPlatillo, @Cantidad, @PrecioUnitario);";

            // Usamos ExecuteAsync porque no necesitamos devolver nada
            await connection.ExecuteAsync(sql, detalle, transaction);
        }
    }
}