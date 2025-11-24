// --- REEMPLAZAR ARCHIVO COMPLETO ---
// Ruta: ./FoodWare/Model/Interfaces/IFinanzasRepository.cs

using FoodWare.Model.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    public interface IFinanzasRepository
    {
        // --- Registro de Datos ---
        Task AgregarGastoAsync(GastoOperativo gasto);
        Task AgregarCorteCajaAsync(CorteCaja corte);

        // --- Consultas para KPIs (Dashboard) ---
        Task<decimal> ObtenerSumaGastosOperativosAsync(DateTime inicio, DateTime fin);
        Task<decimal> CalcularCostoVentasMasMermasAsync(DateTime inicio, DateTime fin);
        Task<decimal> ObtenerEfectivoEsperadoEnCajaAsync(DateTime inicio, DateTime fin);
        Task<decimal> ObtenerVentasTotalesAsync(DateTime inicio, DateTime fin);

        // --- Consultas para Gráficos ---
        Task<List<FlujoFinancieroDto>> ObtenerFlujoFinancieroSemanalAsync(DateTime inicio, DateTime fin);
        Task<List<DesgloseGastosDto>> ObtenerDesgloseDeGastosAsync(DateTime inicio, DateTime fin);
    }
}