using FoodWare.Model.Entities;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    public interface IVentaRepository
    {
        /// <summary>
        /// Agrega una Venta a la BD y devuelve el nuevo ID generado.
        /// </summary>
        Task<int> AgregarVentaAsync(Venta venta, SqlConnection connection, SqlTransaction transaction);

        /// <summary>
        /// Agrega un detalle de venta a la BD.
        /// </summary>
        Task AgregarDetalleAsync(DetalleVenta detalle, SqlConnection connection, SqlTransaction transaction);
    }
}