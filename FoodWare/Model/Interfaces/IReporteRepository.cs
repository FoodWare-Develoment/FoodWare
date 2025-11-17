using FoodWare.Model.Entities;
using System;
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
        /// Obtiene un ranking de los platillos más vendidos por cantidad en un rango de fechas.
        /// </summary>
        Task<List<PlatilloVendidoDto>> ObtenerTopPlatillosVendidosAsync(DateTime inicio, DateTime fin);

        /// <summary>
        /// Obtiene un ranking de prioridad de productos con stock bajo.
        /// (Este no necesita fechas, es un reporte de estado actual)
        /// </summary>
        Task<List<ProductoBajoStockDto>> ObtenerProductosBajoStockAsync();

        /// <summary>
        /// Obtiene el total de ventas agrupado por día en un rango de fechas.
        /// </summary>
        Task<List<ReporteVentasDto>> ObtenerReporteVentasAsync(DateTime inicio, DateTime fin);

        /// <summary>
        /// Obtiene un reporte de pérdidas por mermas en un rango de fechas.
        /// </summary>
        Task<List<ReporteMermasDto>> ObtenerReporteMermasAsync(DateTime inicio, DateTime fin);
    }
}