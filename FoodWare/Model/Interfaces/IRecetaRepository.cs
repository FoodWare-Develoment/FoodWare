using FoodWare.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    public interface IRecetaRepository
    {
        // Lee la receta completa de un platillo
        Task<List<RecetaDetalle>> ObtenerPorPlatilloAsync(int idPlatillo);

        // Añade un nuevo ingrediente a un platillo
        Task AgregarAsync(Receta receta);

        // Elimina un ingrediente de un platillo
        Task EliminarAsync(int idReceta);
    }
}