using Microsoft.AspNetCore.Mvc;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using FoodWare.API.DataAccess; // Necesario para acceder a la configuración
using FoodWare.Shared.Exceptions;

namespace FoodWare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController(
        IVentaRepository ventaRepo,
        IRecetaRepository recetaRepo,
        IProductoRepository productoRepo,
        IMovimientoRepository movimientoRepo,
        IConfiguration config // Para abrir la conexión aquí
        ) : ControllerBase
    {
        private readonly string _connectionString = config.GetConnectionString("FoodWareDB")!;

        [HttpPost]
        public async Task<ActionResult> RegistrarVenta([FromBody] VentaRequest request)
        {
            if (request.Detalles.Count == 0) return BadRequest("La comanda está vacía.");

            // 1. Validar Stock antes de empezar (Lógica de Negocio)
            try
            {
                await VerificarStock(request.Detalles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Stock insuficiente: {ex.Message}");
            }

            // 2. Iniciar Transacción SQL (Todo o Nada)
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                // A. Crear Cabecera de Venta
                var nuevaVenta = new Venta
                {
                    IdUsuario = request.IdUsuario,
                    FormaDePago = request.FormaDePago,
                    TotalVenta = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                    FechaVenta = DateTime.Now
                };

                int idVenta = await ventaRepo.AgregarVentaAsync(nuevaVenta, connection, transaction);

                // B. Guardar Detalles y Descontar Inventario
                foreach (var detalle in request.Detalles)
                {
                    detalle.IdVenta = idVenta;
                    await ventaRepo.AgregarDetalleAsync(detalle, connection, transaction);
                    await DescontarInventario(detalle.IdPlatillo, detalle.Cantidad, idVenta, request.IdUsuario, connection, transaction);
                }

                await transaction.CommitAsync();
                return Ok(new { Mensaje = "Venta registrada exitosamente", IdVenta = idVenta });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error en transacción: {ex.Message}");
            }
        }

        private async Task VerificarStock(List<DetalleVenta> detalles)
        {
            var stockDisponible = await productoRepo.ObtenerMapaStockAsync();
            foreach (var detalle in detalles)
            {
                var receta = await recetaRepo.ObtenerPorPlatilloAsync(detalle.IdPlatillo);
                foreach (var ing in receta)
                {
                    decimal requerido = ing.Cantidad * detalle.Cantidad;
                    if (!stockDisponible.ContainsKey(ing.IdProducto) || stockDisponible[ing.IdProducto] < requerido)
                    {
                        throw new Exception($"Falta {ing.NombreProducto}");
                    }
                    stockDisponible[ing.IdProducto] -= requerido;
                }
            }
        }

        private async Task DescontarInventario(int idPlatillo, int cantidad, int idVenta, int idUsuario, SqlConnection conn, SqlTransaction trans)
        {
            var receta = await recetaRepo.ObtenerPorPlatilloAsync(idPlatillo, conn, trans);
            foreach (var ing in receta)
            {
                var movimiento = new MovimientoInventario
                {
                    IdProducto = ing.IdProducto,
                    IdUsuario = idUsuario,
                    TipoMovimiento = "Venta",
                    Cantidad = -(ing.Cantidad * cantidad),
                    Motivo = $"Venta Móvil #{idVenta}"
                };
                await movimientoRepo.AgregarAsync(movimiento, conn, trans);
            }
        }
    }
}