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
            // Añadimos JOIN a Ventas y filtro de fecha
            string sql = @"
                SELECT 
                    P.Nombre, 
                    SUM(DV.Cantidad) AS TotalVendido
                FROM 
                    DetallesVenta DV
                INNER JOIN 
                    Platillos P ON DV.IdPlatillo = P.IdPlatillo
                INNER JOIN
                    Ventas V ON DV.IdVenta = V.IdVenta
                WHERE
                    V.FechaVenta BETWEEN @Inicio AND @Fin
                GROUP BY 
                    P.Nombre
                ORDER BY 
                    TotalVendido DESC;";

            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<PlatilloVendidoDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }

        public async Task<List<ProductoBajoStockDto>> ObtenerProductosBajoStockAsync()
        {
            string sql = @"
                SELECT 
                    Nombre,
                    StockActual,
                    StockMinimo,
                    (StockMinimo - StockActual) AS CantidadAReordenar,
                    RANK() OVER (
                        ORDER BY 
                            (StockActual / NULLIF(StockMinimo, 0)) ASC
                    ) AS Prioridad
                FROM 
                    Productos
                WHERE 
                    StockActual <= StockMinimo
                ORDER BY 
                    Prioridad ASC;";

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
                FROM 
                    Ventas
                WHERE 
                    FechaVenta BETWEEN @Inicio AND @Fin
                GROUP BY 
                    CAST(FechaVenta AS DATE)
                ORDER BY
                    Dia DESC;";

            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ReporteVentasDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }

        public async Task<List<ReporteMermasDto>> ObtenerReporteMermasAsync(DateTime inicio, DateTime fin)
        {
            // Usamos la tabla MovimientosInventario
            string sql = @"
                SELECT 
                    P.Nombre AS Producto, 
                    M.Motivo, 
                    SUM(M.Cantidad * -1) AS CantidadPerdida,
                    SUM((M.Cantidad * -1) * P.PrecioCosto) AS CostoPerdida
                FROM 
                    MovimientosInventario M
                INNER JOIN 
                    Productos P ON M.IdProducto = P.IdProducto
                WHERE 
                    M.TipoMovimiento = 'Merma' 
                    AND M.Fecha BETWEEN @Inicio AND @Fin
                GROUP BY 
                    P.Nombre, M.Motivo
                ORDER BY
                    CostoPerdida DESC;";

            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<ReporteMermasDto>(sql, new { Inicio = inicio, Fin = fin });
            return [.. reporte];
        }
    }
}