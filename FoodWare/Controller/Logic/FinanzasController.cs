using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodWare.Model.DataAccess;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;

namespace FoodWare.Controller.Logic
{
    public class FinanzasController(IFinanzasRepository finanzasRepo)
    {
        private readonly IFinanzasRepository _finanzasRepo = finanzasRepo;

        /// <summary>
        /// Método MAESTRO: Calcula los 4 KPIs financieros en paralelo.
        /// Implementa concurrencia TPL para no congelar la pantalla.
        /// </summary>
        public async Task<KpiFinanciero> CalcularDashboardAsync(DateTime inicio, DateTime fin)
        {
            var repoConcreto = (FinanzasSqlRepository)_finanzasRepo;

            // --- CONCURRENCIA ---

            // 1. Lanzar Tarea de Ingresos
            var tIngresos = repoConcreto.ObtenerVentasTotalesAsync(inicio, fin);

            // 2. Lanzar Tarea de Costos (Insumos + Mermas)
            var tCostos = _finanzasRepo.CalcularCostoVentasMasMermasAsync(inicio, fin);

            // 3. Lanzar Tarea de Gastos Operativos
            var tGastos = _finanzasRepo.ObtenerSumaGastosOperativosAsync(inicio, fin);

            // 4. Esperar a que TODOS terminen (sin bloquear UI)
            await Task.WhenAll(tIngresos, tCostos, tGastos);

            // --- FIN CONCURRENCIA ---

            // Recolectar resultados
            decimal ingresos = tIngresos.Result;
            decimal costos = tCostos.Result;
            decimal gastos = tGastos.Result;

            // Calcular la Utilidad Neta
            decimal utilidad = ingresos - (costos + gastos);

            return new KpiFinanciero
            {
                IngresosBrutos = ingresos,
                CostoYPerdida = costos,
                GastosOperativos = gastos,
                UtilidadNetaReal = utilidad
            };
        }

        public async Task RegistrarGastoAsync(string concepto, decimal monto, string categoria, DateTime fecha)
        {
            // Validaciones de Integridad
            if (string.IsNullOrWhiteSpace(concepto)) throw new ArgumentException("El concepto es obligatorio.");
            if (monto <= 0) throw new ArgumentException("El monto debe ser mayor a 0.");
            if (string.IsNullOrWhiteSpace(categoria)) throw new ArgumentException("La categoría es obligatoria.");
            if (fecha > DateTime.Now.AddDays(1)) throw new ArgumentException("No puedes registrar gastos futuros.");

            var gasto = new GastoOperativo
            {
                Concepto = concepto,
                Monto = monto,
                Categoria = categoria,
                Fecha = fecha,
                IdUsuario = UserSession.IdUsuario
            };

            await _finanzasRepo.AgregarGastoAsync(gasto);
        }

        /// <summary>
        /// Ejecuta la lógica del Corte Ciego.
        /// Compara lo que dice el sistema vs lo que dice el cajero.
        /// </summary>
        public async Task<CorteCaja> RealizarCorteCiegoAsync(decimal totalContado, DateTime inicioTurno)
        {
            if (totalContado < 0) throw new ArgumentException("El efectivo contado no puede ser negativo.");

            // 1. Obtener cuánto debería haber (Ventas en Efectivo)
            // Nota: En un sistema real, 'inicioTurno' vendría de la tabla de Turnos.
            // Aquí usaremos el inicio del día.
            decimal esperado = await _finanzasRepo.ObtenerEfectivoEsperadoEnCajaAsync(inicioTurno, DateTime.Now);

            // 2. Calcular Diferencia
            decimal diferencia = totalContado - esperado;
            string estado = "Cuadrado";

            if (diferencia > 0) estado = "Sobrante";
            else if (diferencia < 0) estado = "Faltante";

            // 3. Crear el objeto
            var corte = new CorteCaja
            {
                FechaCierre = DateTime.Now,
                IdUsuarioCajero = UserSession.IdUsuario,
                FondoInicial = 0,
                TotalEfectivoEsperado = esperado,
                TotalEfectivoContado = totalContado,
                Diferencia = diferencia,
                SobranteFaltante = estado
            };

            // 4. Guardar el reporte inmutable
            await _finanzasRepo.AgregarCorteCajaAsync(corte);

            return corte; // Devolvemos el corte para mostrar el resultado en UI
        }

        // Métodos para gráficos
        public async Task<List<DesgloseGastosDto>> ObtenerDatosGraficoPastelAsync(DateTime inicio, DateTime fin)
        {
            return await _finanzasRepo.ObtenerDesgloseDeGastosAsync(inicio, fin);
        }
    }
}