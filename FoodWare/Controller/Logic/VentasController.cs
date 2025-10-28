using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FoodWare.Controller.Logic
{
    public class VentasController(IVentaRepository ventaRepo, IRecetaRepository recetaRepo, IProductoRepository productoRepo)
    {
        private readonly IVentaRepository _ventaRepo = ventaRepo;
        private readonly IRecetaRepository _recetaRepo = recetaRepo;
        private readonly IProductoRepository _productoRepo = productoRepo;
        private readonly string _connectionString = Program.Configuration.GetConnectionString("FoodWareDB")!;

        /// <summary>
        /// Registra una venta completa como una transacción atómica.
        /// </summary>
        public async Task RegistrarVentaAsync(Venta venta, List<DetalleVenta> detalles)
        {
            // 1. Abrir la conexión y COMENZAR TRANSACCIÓN
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                // 2. Guardar la Venta principal y obtener su ID
                int idVenta = await _ventaRepo.AgregarVentaAsync(venta, connection, transaction);

                // 3. Iterar sobre los detalles de la venta
                foreach (var detalle in detalles)
                {
                    detalle.IdVenta = idVenta; // Asignar el ID de la venta padre

                    // 3a. Guardar el detalle de la venta
                    await _ventaRepo.AgregarDetalleAsync(detalle, connection, transaction);

                    // 3b. Descontar el inventario (El núcleo)
                    await DescontarInventarioPorPlatilloAsync(detalle.IdPlatillo, detalle.Cantidad, connection, transaction);
                }

                // 4. Si todo salió bien, COMMIT (confirmar cambios)
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // 5. Si algo falló, ROLLBACK (deshacer cambios)
                await transaction.RollbackAsync();
                throw; // Relanza la excepción para que la Vista la atrape
            }
        }

        /// <summary>
        /// Helper privado para descontar ingredientes de un platillo vendido.
        /// </summary>
        private async Task DescontarInventarioPorPlatilloAsync(int idPlatillo, int cantidadVendida, SqlConnection connection, SqlTransaction transaction)
        {
            // 1. Obtener la receta del platillo
            var receta = await _recetaRepo.ObtenerPorPlatilloAsync(idPlatillo, connection, transaction);

            // 2. Iterar sobre cada ingrediente de la receta
            foreach (var ingrediente in receta)
            {
                decimal cantidadADescontar = ingrediente.Cantidad * cantidadVendida;

                // 3. Actualizar el stock del producto
                await _productoRepo.ActualizarStockAsync(ingrediente.IdProducto, cantidadADescontar, connection, transaction);
            }
        }
    }
}