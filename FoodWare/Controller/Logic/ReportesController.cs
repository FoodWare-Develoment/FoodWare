using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Controller.Logic
{
    public class ReportesController(IReporteRepository repositorio)
    {
        private readonly IReporteRepository _repositorio = repositorio;

        public async Task<List<PlatilloVendidoDto>> CargarTopPlatillosVendidosAsync()
        {
            return await _repositorio.ObtenerTopPlatillosVendidosAsync();
        }

        public async Task<List<ProductoBajoStockDto>> CargarReporteStockBajoAsync()
        {
            return await _repositorio.ObtenerProductosBajoStockAsync();
        }
    }
}