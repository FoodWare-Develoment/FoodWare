using Dapper;
using FoodWare.Controller;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// (Funcionalidad 1: Agregación y Agrupación)
        /// Obtiene un ranking de platillos ordenados por el total de unidades vendidas.
        /// </summary>
        public async Task<List<PlatilloVendidoDto>> ObtenerTopPlatillosVendidosAsync()
        {
            // Esta consulta usa:
            // 1. JOIN: Para conectar DetallesVenta con Platillos.
            // 2. SUM(): Función de agregación para sumar las cantidades.
            // 3. GROUP BY: Para agrupar los resultados por nombre de platillo.
            // 4. ORDER BY: Ordena automáticamente de mayor a menor.
            string sql = @"
                SELECT 
                    P.Nombre, 
                    SUM(DV.Cantidad) AS TotalVendido
                FROM 
                    DetallesVenta DV
                INNER JOIN 
                    Platillos P ON DV.IdPlatillo = P.IdPlatillo
                GROUP BY 
                    P.Nombre
                ORDER BY 
                    TotalVendido DESC;";

            using var connection = new SqlConnection(_connectionString);
            var reporte = await connection.QueryAsync<PlatilloVendidoDto>(sql);
            return [.. reporte];
        }

        /// <summary>
        /// (Funcionalidad 2: Función de Ventana)
        /// Obtiene productos que necesitan reabastecimiento, ordenados por prioridad.
        /// </summary>
        public async Task<List<ProductoBajoStockDto>> ObtenerProductosBajoStockAsync()
        {
            // Esta consulta usa:
            // 1. WHERE: Para filtrar solo productos bajo el mínimo.
            // 2. Cálculo: (StockMinimo - StockActual) para la necesidad.
            // 3. NULLIF: Para prevenir división por cero si StockMinimo es 0.
            // 4. RANK() OVER(): Función de ventana avanzada para asignar un ranking.
            // 5. ORDER BY: Ordena automáticamente por la prioridad (ranking).
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
    }
}