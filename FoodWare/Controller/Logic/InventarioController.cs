using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess;

namespace FoodWare.Controller.Logic
{
    public class InventarioController(IProductoRepository repositorio)
    {
        // solo conozca la INTERFAZ (la idea de un repositorio).
        private readonly IProductoRepository _repositorio = repositorio;

        // El controlador expone los métodos que la Vista necesita
        /// <summary>
        /// Carga la lista de productos de forma asíncrona.
        /// </summary>
        public async Task<List<Producto>> CargarProductosAsync()
        {
            // Simplemente esperamos el resultado del repositorio
            return await _repositorio.ObtenerTodosAsync();
        }

        /// <summary>
        /// Guarda un nuevo producto en el repositorio.
        /// </summary>
        /// <param name="nombre">Nombre del producto. No puede ser nulo o vacío.</param>
        /// <param name="categoria">Categoría del producto.</param>
        /// <param name="stock">Cantidad inicial en stock. No puede ser negativo.</param>
        /// <param name="precio">Precio de costo del producto. No puede ser negativo.</param>
        /// <exception cref="ArgumentException">Se lanza si el nombre está vacío o si el stock o precio son negativos.</exception>
        /// <summary>
        /// Guarda un nuevo producto en el repositorio de forma asíncrona.
        /// </summary>
        public async Task GuardarNuevoProductoAsync(string nombre, string categoria, string unidad, decimal stock, decimal stockminimo, decimal precio)
        {
            // Las validaciones ahora son más específicas.
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(nombre));
            }

            if (string.IsNullOrWhiteSpace(categoria))
            {
                throw new ArgumentException("La categoria del producto no puede estar vacío.", nameof(categoria));
            }

            if (string.IsNullOrWhiteSpace(unidad))
            {
                throw new ArgumentException("La unidad del producto no puede estar vacío.", nameof(unidad));
            }

            if (stock < 0 || stockminimo < 0 || precio < 0)
            {
                throw new ArgumentException("Los valores numéricos no pueden ser negativos.");
            }

            Producto nuevo = new()
            {
                Nombre = nombre,
                Categoria = categoria,
                UnidadMedida = unidad, // <-- EL CAMPO QUE FALTABA
                StockActual = stock,
                StockMinimo = stockminimo,
                PrecioCosto = precio
            };

            // Llamamos al método asíncrono del repositorio
            await _repositorio.AgregarAsync(nuevo);
        }

        /// <summary>
        /// Elimina un producto de forma asíncrona.
        /// </summary>
        public async Task EliminarProductoAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }

        // Aquí iran métodos para Actualizar...
    }
}