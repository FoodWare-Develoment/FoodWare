using Dapper;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    public class VentaSqlRepository : IVentaRepository
    {
        public async Task<int> AgregarVentaAsync(Venta venta, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"INSERT INTO Ventas (FechaVenta, IdUsuario, FormaDePago, TotalVenta) 
                           VALUES (@FechaVenta, @IdUsuario, @FormaDePago, @TotalVenta);
                           SELECT SCOPE_IDENTITY();"; // <-- Devuelve el ID generado

            // ExecuteScalarAsync devuelve el primer valor de la primera fila (el nuevo ID)
            return await connection.ExecuteScalarAsync<int>(sql, venta, transaction);
        }

        public async Task AgregarDetalleAsync(DetalleVenta detalle, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"INSERT INTO DetallesVenta (IdVenta, IdPlatillo, Cantidad, PrecioUnitario) 
                           VALUES (@IdVenta, @IdPlatillo, @Cantidad, @PrecioUnitario);";

            await connection.ExecuteAsync(sql, detalle, transaction);
        }
    }
}