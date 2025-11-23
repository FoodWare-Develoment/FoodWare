using Dapper;
using FoodWare.Controller;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    public class ReporteSqlRepository : IReporteRepository
    {
        private readonly string _connectionString;

        public ReporteSqlRepository()
        {
            _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task<List<PlatilloVendidoDto>> ObtenerTopPlatillosVendidosAsync(DateTime inicio, DateTime fin)
        {
            string sql = @"
                SELECT 
                    P.Nombre, 
                    SUM(DV.Cantidad) AS TotalVendido
                FROM DetallesVenta DV
                INNER JOIN Platillos P ON DV.IdPlatillo = P.IdPlatillo
                INNER JOIN Ventas V ON DV.IdVenta = V.IdVenta
                WHERE V.FechaVenta BETWEEN @Inicio AND @Fin
                GROUP BY P.Nombre
                ORDER BY TotalVendido DESC;";
            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<PlatilloVendidoDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }

        public async Task<List<ProductoBajoStockDto>> ObtenerProductosBajoStockAsync()
        {
            string sql = @"
                SELECT 
                    Nombre, StockActual, StockMinimo,
                    (StockMinimo - StockActual) AS CantidadAReordenar,
                    RANK() OVER (ORDER BY (StockActual / NULLIF(StockMinimo, 0)) ASC) AS Prioridad
                FROM Productos
                WHERE StockActual <= StockMinimo
                ORDER BY Prioridad ASC;";
            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ProductoBajoStockDto>(sql);
            return [.. reporte];
        }

        public async Task<List<ReporteVentasDto>> ObtenerReporteVentasAsync(DateTime inicio, DateTime fin)
        {
            string sql = @"
                SELECT 
                    CAST(FechaVenta AS DATE) AS Dia, 
                    SUM(TotalVenta) AS TotalDiario
                FROM Ventas
                WHERE FechaVenta BETWEEN @Inicio AND @Fin
                GROUP BY CAST(FechaVenta AS DATE)
                ORDER BY Dia DESC;";
            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ReporteVentasDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }

        public async Task<List<ReporteMermasDto>> ObtenerReporteMermasAsync(DateTime inicio, DateTime fin)
        {
            string sql = @"
                SELECT 
                    P.Nombre AS Producto, 
                    M.Motivo, 
                    SUM(M.Cantidad * -1) AS CantidadPerdida,
                    SUM((M.Cantidad * -1) * P.PrecioCosto) AS CostoPerdida
                FROM MovimientosInventario M
                INNER JOIN Productos P ON M.IdProducto = P.IdProducto
                WHERE M.TipoMovimiento = 'Merma' 
                AND M.Fecha BETWEEN @Inicio AND @Fin
                GROUP BY P.Nombre, M.Motivo
                ORDER BY CostoPerdida DESC;";
            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ReporteMermasDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }

        public async Task<List<ReporteRentabilidadDto>> ObtenerReporteRentabilidadAsync(DateTime inicio, DateTime fin)
        {
            // Primero calcula el costo de cada receta,
            // luego lo junta con las ventas.
            string sql = @"
                ;WITH CostoReceta AS (
                    -- 1. Calcular el costo de cada platillo
                    SELECT 
                        R.IdPlatillo,
                        SUM(R.Cantidad * P.PrecioCosto) AS CostoReceta
                    FROM 
                        Recetas R
                    INNER JOIN 
                        Productos P ON R.IdProducto = P.IdProducto
                    GROUP BY 
                        R.IdPlatillo
                ),
                VentasPlatillo AS (
                    -- 2. Contar las unidades vendidas en el rango de fechas
                    SELECT
                        DV.IdPlatillo,
                        SUM(DV.Cantidad) AS UnidadesVendidas
                    FROM
                        DetallesVenta DV
                    INNER JOIN
                        Ventas V ON DV.IdVenta = V.IdVenta
                    WHERE
                        V.FechaVenta BETWEEN @Inicio AND @Fin
                    GROUP BY
                        DV.IdPlatillo
                )
                -- 3. Unir todo y calcular la rentabilidad
                SELECT 
                    P.Nombre AS Platillo,
                    P.PrecioVenta,
                    ISNULL(CR.CostoReceta, 0) AS CostoReceta,
                    (P.PrecioVenta - ISNULL(CR.CostoReceta, 0)) AS GananciaBruta,
                    ISNULL(VP.UnidadesVendidas, 0) AS UnidadesVendidas,
                    (P.PrecioVenta - ISNULL(CR.CostoReceta, 0)) * ISNULL(VP.UnidadesVendidas, 0) AS GananciaTotal
                FROM 
                    Platillos P
                LEFT JOIN 
                    CostoReceta CR ON P.IdPlatillo = CR.IdPlatillo
                LEFT JOIN
                    VentasPlatillo VP ON P.IdPlatillo = VP.IdPlatillo
                ORDER BY
                    GananciaTotal DESC;";

            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ReporteRentabilidadDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }

        public async Task<List<ReporteVentasHoraDto>> ObtenerReporteVentasPorHoraAsync(DateTime inicio, DateTime fin)
        {
            string sql = @"
                SELECT 
                    DATEPART(hour, FechaVenta) AS Hora, 
                    SUM(TotalVenta) AS TotalVendido,
                    COUNT(IdVenta) AS NumeroVentas
                FROM 
                    Ventas
                WHERE 
                    FechaVenta BETWEEN @Inicio AND @Fin
                GROUP BY 
                    DATEPART(hour, FechaVenta)
                ORDER BY
                    Hora ASC;";

            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ReporteVentasHoraDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }
    }
}