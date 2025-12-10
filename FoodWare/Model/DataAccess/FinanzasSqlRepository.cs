using Dapper;
using FoodWare.Controller;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    public class FinanzasSqlRepository : IFinanzasRepository
    {
        private readonly string _connectionString;

        public FinanzasSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        // --- REGISTRO DE DATOS ---

        public async Task AgregarGastoAsync(GastoOperativo gasto)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                INSERT INTO GastosOperativos (Concepto, Monto, Categoria, Fecha, IdUsuario)
                VALUES (@Concepto, @Monto, @Categoria, @Fecha, @IdUsuario);";
            await connection.ExecuteAsync(sql, gasto);
        }

        public async Task AgregarCorteCajaAsync(CorteCaja corte)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                INSERT INTO CortesCaja 
                (FechaCierre, IdUsuarioCajero, FondoInicial, TotalEfectivoEsperado, TotalEfectivoContado, Diferencia, SobranteFaltante)
                VALUES 
                (@FechaCierre, @IdUsuarioCajero, @FondoInicial, @TotalEfectivoEsperado, @TotalEfectivoContado, @Diferencia, @SobranteFaltante);";
            await connection.ExecuteAsync(sql, corte);
        }

        // --- CONSULTAS PARA KPIs (DASHBOARD) ---

        public async Task<decimal> ObtenerSumaGastosOperativosAsync(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT ISNULL(SUM(Monto), 0) FROM GastosOperativos WHERE Fecha BETWEEN @Inicio AND @Fin;";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { Inicio = inicio, Fin = fin });
        }

        /// <summary>
        /// Esta es la consulta más compleja. Suma dos cosas:
        /// 1. El costo de los ingredientes de todo lo vendido.
        /// 2. El costo de las mermas registradas.
        /// </summary>
        public async Task<decimal> CalcularCostoVentasMasMermasAsync(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                SELECT 
                    (
                        -- PARTE A: Costo de lo vendido (Recetas)
                        SELECT ISNULL(SUM(DV.Cantidad * R.Cantidad * P.PrecioCosto), 0)
                        FROM DetallesVenta DV
                        INNER JOIN Ventas V ON DV.IdVenta = V.IdVenta
                        INNER JOIN Recetas R ON DV.IdPlatillo = R.IdPlatillo
                        INNER JOIN Productos P ON R.IdProducto = P.IdProducto
                        WHERE V.FechaVenta BETWEEN @Inicio AND @Fin
                    ) 
                    + 
                    (
                        -- PARTE B: Costo de Mermas (Movimientos tipo 'Merma')
                        -- Multiplicamos por -1 porque en la BD las mermas se guardan en negativo
                        SELECT ISNULL(SUM(M.Cantidad * -1 * P.PrecioCosto), 0)
                        FROM MovimientosInventario M
                        INNER JOIN Productos P ON M.IdProducto = P.IdProducto
                        WHERE M.TipoMovimiento = 'Merma' AND M.Fecha BETWEEN @Inicio AND @Fin
                    ) AS CostoTotalLogistico";

            return await connection.ExecuteScalarAsync<decimal>(sql, new { Inicio = inicio, Fin = fin });
        }

        public async Task<decimal> ObtenerEfectivoEsperadoEnCajaAsync(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                SELECT ISNULL(SUM(TotalVenta), 0) 
                FROM Ventas 
                WHERE FormaDePago = 'Efectivo' AND FechaVenta BETWEEN @Inicio AND @Fin;";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { Inicio = inicio, Fin = fin });
        }

        // --- CONSULTAS PARA GRÁFICOS ---

        public async Task<List<FlujoFinancieroDto>> ObtenerFlujoFinancieroSemanalAsync(DateTime inicio, DateTime fin)
        {
            // Esta consulta agrupa por día y compara Ingresos (Ventas) vs Egresos (Gastos + Costos)
            // Nota: Es una aproximación simplificada para el gráfico.
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                SELECT 
                    CAST(V.FechaVenta AS DATE) AS Fecha,
                    SUM(V.TotalVenta) AS IngresoTotal,
                    0 AS EgresoTotal -- Se llenará en lógica de negocio o query más avanzado
                FROM Ventas V
                WHERE V.FechaVenta BETWEEN @Inicio AND @Fin
                GROUP BY CAST(V.FechaVenta AS DATE)
                ORDER BY Fecha;";

            // Para simplificar el gráfico en V1, solo traeremos ventas diarias como Ingreso.
            // Los egresos diarios son complejos de calcular en SQL puro sin tablas temporales.
            // Dejaremos EgresoTotal en 0 y lo gestionaremos si es crítico, o mostramos Ventas vs Gastos Op.

            var resultado = await connection.QueryAsync<FlujoFinancieroDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. resultado];
        }

        public async Task<List<DesgloseGastosDto>> ObtenerDesgloseDeGastosAsync(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                -- 1. Gastos Operativos Agrupados
                SELECT Categoria, SUM(Monto) AS MontoTotal 
                FROM GastosOperativos 
                WHERE Fecha BETWEEN @Inicio AND @Fin
                GROUP BY Categoria
                
                UNION ALL
                
                -- 2. Costo de Insumos (Totalizado como una categoría)
                SELECT 'Insumos (Recetas)' AS Categoria, 
                       ISNULL(SUM(DV.Cantidad * R.Cantidad * P.PrecioCosto), 0) AS MontoTotal
                FROM DetallesVenta DV
                INNER JOIN Ventas V ON DV.IdVenta = V.IdVenta
                INNER JOIN Recetas R ON DV.IdPlatillo = R.IdPlatillo
                INNER JOIN Productos P ON R.IdProducto = P.IdProducto
                WHERE V.FechaVenta BETWEEN @Inicio AND @Fin

                UNION ALL

                -- 3. Mermas (Totalizado)
                SELECT 'Mermas (Desperdicio)' AS Categoria,
                       ISNULL(SUM(M.Cantidad * -1 * P.PrecioCosto), 0) AS MontoTotal
                FROM MovimientosInventario M
                INNER JOIN Productos P ON M.IdProducto = P.IdProducto
                WHERE M.TipoMovimiento = 'Merma' AND M.Fecha BETWEEN @Inicio AND @Fin;";

            var resultado = await connection.QueryAsync<DesgloseGastosDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. resultado];
        }

        // Método auxiliar que necesitamos para el Controller (Ingresos Brutos)
        public async Task<decimal> ObtenerVentasTotalesAsync(DateTime inicio, DateTime fin)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT ISNULL(SUM(TotalVenta), 0) FROM Ventas WHERE FechaVenta BETWEEN @Inicio AND @Fin;";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { Inicio = inicio, Fin = fin });
        }
    }
}