using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Controller.Logic
{
    public class ReportesController(IReporteRepository repositorio)
    {
        private readonly IReporteRepository _repositorio = repositorio;

        public async Task<List<PlatilloVendidoDto>> CargarTopPlatillosVendidosAsync(DateTime inicio, DateTime fin)
        {
            return await _repositorio.ObtenerTopPlatillosVendidosAsync(inicio, fin);
        }

        public async Task<List<ProductoBajoStockDto>> CargarReporteStockBajoAsync()
        {
            return await _repositorio.ObtenerProductosBajoStockAsync();
        }

        public async Task<List<ReporteVentasDto>> CargarReporteVentasAsync(DateTime inicio, DateTime fin)
        {
            return await _repositorio.ObtenerReporteVentasAsync(inicio, fin);
        }

        public async Task<List<ReporteMermasDto>> CargarReporteMermasAsync(DateTime inicio, DateTime fin)
        {
            return await _repositorio.ObtenerReporteMermasAsync(inicio, fin);
        }
    }
}