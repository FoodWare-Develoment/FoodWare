using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodWare.Controller.Exceptionss;
using FoodWare.Controller.Logic;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FoodWare.Controller.Logic
{
    public class VentasController(
        IVentaRepository ventaRepo,
        IRecetaRepository recetaRepo,
        IProductoRepository productoRepo,
        IMovimientoRepository movimientoRepo
        )
    {
        private readonly IVentaRepository _ventaRepo = ventaRepo;
        private readonly IRecetaRepository _recetaRepo = recetaRepo;
        private readonly IProductoRepository _productoRepo = productoRepo;
        private readonly IMovimientoRepository _movimientoRepo = movimientoRepo;
        private readonly string _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;

        public async Task RegistrarVentaAsync(Venta venta, List<DetalleVenta> detalles)
        {
            await VerificarDisponibilidadStockAsync(detalles);

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                int idVenta = await _ventaRepo.AgregarVentaAsync(venta, connection, transaction);

                foreach (var detalle in detalles)
                {
                    detalle.IdVenta = idVenta;
                    await _ventaRepo.AgregarDetalleAsync(detalle, connection, transaction);

                    await DescontarInventarioPorPlatilloAsync(detalle.IdPlatillo, detalle.Cantidad, idVenta, venta.IdUsuario, connection, transaction);
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Registra los movimientos de inventario (salidas) para un platillo vendido.
        /// </summary>
        private async Task DescontarInventarioPorPlatilloAsync(int idPlatillo, int cantidadVendida, int idVenta, int idUsuario, SqlConnection connection, SqlTransaction transaction)
        {
            var receta = await _recetaRepo.ObtenerPorPlatilloAsync(idPlatillo, connection, transaction);

            foreach (var ingrediente in receta)
            {
                decimal cantidadADescontar = ingrediente.Cantidad * cantidadVendida;

                MovimientoInventario movimiento = new()
                {
                    IdProducto = ingrediente.IdProducto,
                    IdUsuario = idUsuario,
                    TipoMovimiento = "Venta",
                    Cantidad = -cantidadADescontar,
                    Motivo = $"Venta ID: {idVenta}"
                };

                await _movimientoRepo.AgregarAsync(movimiento, connection, transaction);
            }
        }

        /// <summary>
        /// Verifica la comanda contra el stock actual. Lanza una StockInsuficienteException si falla.
        /// </summary>
        private async Task VerificarDisponibilidadStockAsync(List<DetalleVenta> detalles)
        {
            var stockDisponible = await _productoRepo.ObtenerMapaStockAsync();

            foreach (var detalle in detalles)
            {
                var receta = await _recetaRepo.ObtenerPorPlatilloAsync(detalle.IdPlatillo);

                foreach (var ingrediente in receta)
                {
                    decimal cantidadRequerida = ingrediente.Cantidad * detalle.Cantidad;

                    if (!stockDisponible.TryGetValue(ingrediente.IdProducto, out decimal stockActual) || cantidadRequerida > stockActual)
                    {
                        int maxPosible = 0;
                        if (ingrediente.Cantidad > 0)
                        {
                            maxPosible = (int)Math.Floor(stockActual / ingrediente.Cantidad);
                        }

                        throw new StockInsuficienteException(
                            $"Stock insuficiente para '{ingrediente.NombreProducto}'.",
                            detalle.NombrePlatillo,
                            ingrediente.NombreProducto,
                            maxPosible);
                    }
                    else
                    {
                        stockDisponible[ingrediente.IdProducto] -= cantidadRequerida;
                    }
                }
            }
        }
    }
}