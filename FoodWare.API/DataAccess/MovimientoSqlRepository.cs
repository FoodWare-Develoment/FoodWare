using Dapper;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FoodWare.API.DataAccess
{
    public class MovimientoSqlRepository(IConfiguration configuration) : IMovimientoRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("FoodWareDB")!;
        private const string INSERT_SQL = @"
            INSERT INTO MovimientosInventario 
                (IdProducto, IdUsuario, Fecha, TipoMovimiento, Cantidad, Motivo) 
            VALUES 
                (@IdProducto, @IdUsuario, @Fecha, @TipoMovimiento, @Cantidad, @Motivo);";

        public async Task AgregarAsync(MovimientoInventario movimiento)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(INSERT_SQL, movimiento);
            // El trigger TR_ActualizarStockProducto se encargará de actualizar [Productos]
        }

        public async Task AgregarAsync(MovimientoInventario movimiento, SqlConnection connection, SqlTransaction transaction)
        {
            // Este método usa la conexión y transacción existentes (para el TPV)
            await connection.ExecuteAsync(INSERT_SQL, movimiento, transaction);
            // El trigger TR_ActualizarStockProducto se encargará de actualizar [Productos]
        }
    }
}