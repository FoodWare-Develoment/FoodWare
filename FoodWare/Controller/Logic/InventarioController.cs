using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using FoodWare.Model.DataAccess;

namespace FoodWare.Controller.Logic
{
    public class InventarioController(
        IProductoRepository repositorio,
        IMovimientoRepository movimientoRepo,
        IRecetaRepository recetaRepo
        )
    {
        private readonly IProductoRepository _repositorio = repositorio;
        private readonly IMovimientoRepository _movimientoRepo = movimientoRepo; 
        private readonly IRecetaRepository _recetaRepo = recetaRepo; 

        public async Task<List<Producto>> CargarProductosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        public async Task GuardarNuevoProductoAsync(string nombre, string categoria, string unidad, decimal stock, decimal stockminimo, decimal precio)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(nombre));

            Producto nuevo = new()
            {
                Nombre = nombre,
                Categoria = categoria,
                UnidadMedida = unidad,
                StockActual = stock, 
                StockMinimo = stockminimo,
                PrecioCosto = precio
            };
            await _repositorio.AgregarAsync(nuevo);

            if (stock > 0)
            {
                // Necesitamos el ID del producto que acabamos de crear...
                // Esto es más complejo. Por ahora, asumimos que el stock inicial
                // se registra y luego se añade con "Añadir Stock".
                // Para simplificar, cambia la lógica de Guardar:
                // 1. Guarda el producto con StockActual = 0
                // 2. Llama a RegistrarEntradaAsync(idNuevo, stock, idUsuario, "Ajuste Inicial")
            }
        }

        public async Task EliminarProductoAsync(int id)
        {
            if (await _recetaRepo.ProductoEstaEnUsoAsync(id))
            {
                throw new InvalidOperationException("El producto no puede ser eliminado. Está en uso en una o más recetas.");
            }

            await _repositorio.EliminarAsync(id);
        }

        public async Task ActualizarProductoAsync(Producto producto)
        {
            await _repositorio.ActualizarAsync(producto);
        }

        public async Task RegistrarEntradaAsync(int idProducto, int idUsuario, decimal cantidad, string motivo)
        {
            if (idProducto <= 0)
                throw new ArgumentException("Producto no válido.");
            if (idUsuario <= 0)
                throw new ArgumentException("Usuario no válido.");
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad a añadir debe ser mayor a cero.");

            MovimientoInventario movimiento = new()
            {
                IdProducto = idProducto,
                IdUsuario = idUsuario,
                TipoMovimiento = "Entrada",
                Cantidad = cantidad, // Positivo
                Motivo = motivo
            };
            await _movimientoRepo.AgregarAsync(movimiento);
        }

        // --- Mermas ---
        public async Task RegistrarMermaAsync(int idProducto, int idUsuario, decimal cantidad, string motivo)
        {
            if (idProducto <= 0)
                throw new ArgumentException("Producto no válido.");
            if (idUsuario <= 0)
                throw new ArgumentException("Usuario no válido.");
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad de merma debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de la merma es obligatorio.");

            MovimientoInventario movimiento = new()
            {
                IdProducto = idProducto,
                IdUsuario = idUsuario,
                TipoMovimiento = "Merma",
                Cantidad = -cantidad, // Negativo
                Motivo = motivo
            };
            await _movimientoRepo.AgregarAsync(movimiento);
        }
    }
}