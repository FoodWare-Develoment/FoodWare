using FoodWare.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Shared.Interfaces
{
    public interface IReporteRepository
    {
        Task<List<PlatilloVendidoDto>> ObtenerTopPlatillosVendidosAsync(DateTime inicio, DateTime fin);
        Task<List<ProductoBajoStockDto>> ObtenerProductosBajoStockAsync();
        Task<List<ReporteVentasDto>> ObtenerReporteVentasAsync(DateTime inicio, DateTime fin);
        Task<List<ReporteMermasDto>> ObtenerReporteMermasAsync(DateTime inicio, DateTime fin);
        Task<List<ReporteRentabilidadDto>> ObtenerReporteRentabilidadAsync(DateTime inicio, DateTime fin);
        Task<List<ReporteVentasHoraDto>> ObtenerReporteVentasPorHoraAsync(DateTime inicio, DateTime fin);
    }
}