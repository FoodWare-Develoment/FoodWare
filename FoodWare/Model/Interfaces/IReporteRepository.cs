using FoodWare.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    /// <summary>
    /// Define las operaciones de solo lectura para obtener reportes complejos.
    /// </summary>
    public interface IReporteRepository
    {
        /// <summary>
        /// Obtiene un ranking de los platillos más vendidos por cantidad.
        /// </summary>
        Task<List<PlatilloVendidoDto>> ObtenerTopPlatillosVendidosAsync();

        /// <summary>
        /// Obtiene un ranking de prioridad de productos con stock bajo.
        /// </summary>
        Task<List<ProductoBajoStockDto>> ObtenerProductosBajoStockAsync();
    }
}