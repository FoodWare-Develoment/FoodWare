using FoodWare.Model.Entities;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    public interface IRecetaRepository
    {
        // Lee la receta completa de un platillo
        Task<List<RecetaDetalle>> ObtenerPorPlatilloAsync(int idPlatillo);
        Task<List<RecetaDetalle>> ObtenerPorPlatilloAsync(int idPlatillo, SqlConnection connection, SqlTransaction transaction);

        // Añade un nuevo ingrediente a un platillo
        Task AgregarAsync(Receta receta);

        // Elimina un ingrediente de un platillo
        Task EliminarAsync(int idReceta);

        /// <summary>
        /// Verifica si un producto (ingrediente) está siendo usado en alguna receta.
        /// </summary>
        Task<bool> ProductoEstaEnUsoAsync(int idProducto);
    }
}