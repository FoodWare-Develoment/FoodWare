using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess; // Necesario para crear el Mock

namespace FoodWare.Controller.Logic
{
    public class InventarioController(IProductoRepository repositorio)
    {
        // solo conozca la INTERFAZ (la idea de un repositorio).
        private readonly IProductoRepository _repositorio = repositorio;

        // El controlador expone los métodos que la Vista necesita
        public List<Producto> CargarProductos()
        {
            return _repositorio.ObtenerTodos();
        }

        /// <summary>
        /// Guarda un nuevo producto en el repositorio.
        /// </summary>
        /// <param name="nombre">Nombre del producto. No puede ser nulo o vacío.</param>
        /// <param name="categoria">Categoría del producto.</param>
        /// <param name="stock">Cantidad inicial en stock. No puede ser negativo.</param>
        /// <param name="precio">Precio de costo del producto. No puede ser negativo.</param>
        /// <exception cref="ArgumentException">Se lanza si el nombre está vacío o si el stock o precio son negativos.</exception>
        public void GuardarNuevoProducto(string nombre, string categoria, string unidad, decimal stock, decimal stockminimo, decimal precio)
        {
            // Las validaciones ahora son más específicas.
            if (string.IsNullOrWhiteSpace(nombre))
            {
                // Esta excepción indica que un argumento (parámetro) tiene un valor inválido.
                throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(nombre));
            }

            if (string.IsNullOrWhiteSpace(categoria))
            {
                // Esta excepción indica que un argumento (parámetro) tiene un valor inválido.
                throw new ArgumentException("La categoria del producto no puede estar vacío.", nameof(categoria));
            }

            if (string.IsNullOrWhiteSpace(unidad))
            {
                // Esta excepción indica que un argumento (parámetro) tiene un valor inválido.
                throw new ArgumentException("La unidad del producto no puede estar vacío.", nameof(unidad));
            }

            if (stock < 0)
            {
                throw new ArgumentException("El stock no puede ser un valor negativo.", nameof(stock));
            }

            if (stockminimo < 0)
            {
                throw new ArgumentException("El stockminimo no puede ser un valor negativo.", nameof(stockminimo));
            }

            if (precio < 0)
            {
                throw new ArgumentException("El precio no puede ser un valor negativo.", nameof(precio));
            }

            Producto nuevo = new()
            {
                Nombre = nombre,
                Categoria = categoria,
                UnidadMedida = unidad,
                StockActual = stock,
                StockMinimo = stockminimo,
                PrecioCosto = precio
            };
            _repositorio.Agregar(nuevo);
        }

        public void EliminarProducto(int id)
        {
            _repositorio.Eliminar(id);
        }

        // Aquí iran métodos para Actualizar...
    }
}