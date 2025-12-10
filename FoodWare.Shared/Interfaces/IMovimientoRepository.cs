using FoodWare.Shared.Entities;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FoodWare.Shared.Interfaces
{
    public interface IMovimientoRepository
    {
        /// <summary>
        /// Agrega un nuevo movimiento de inventario (positivo o negativo)
        /// </summary>
        Task AgregarAsync(MovimientoInventario movimiento);

        /// <summary>
        /// Agrega un nuevo movimiento usando una transacción existente (para ventas)
        /// </summary>
        Task AgregarAsync(MovimientoInventario movimiento, SqlConnection connection, SqlTransaction transaction);
    }
}