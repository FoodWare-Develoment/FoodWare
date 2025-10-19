using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess;

namespace FoodWare.Controller.Logic
{
    public class MenuController(IPlatilloRepository repositorio)
    {
        // solo conozca la INTERFAZ (la idea de un repositorio).
        private readonly IPlatilloRepository _repositorio = repositorio;

        /// <summary>
        /// Carga los platillos de forma asíncrona.
        /// </summary>
        public async Task<List<Platillo>> CargarPlatillosAsync()
        {
            return await _repositorio.ObtenerTodosAsync();
        }

        /// <summary>
        /// Guarda un nuevo platillo en el repositorio.
        /// </summary>
        /// <param name="nombre">Nombre del producto. No puede ser nulo o vacío.</param>
        /// <param name="categoria">Categoría del producto.</param>
        /// <param name="precio">Precio de costo del platillo. No puede ser negativo.</param>
        /// <exception cref="ArgumentException">Se lanza si el nombre o categoria está vacío o si el precio son negativos.</exception>
        /// <summary>
        /// Guarda un nuevo platillo en el repositorio de forma asíncrona.
        /// </summary>
        public async Task GuardarNuevoPlatilloAsync(string nombre, string categoria, decimal precio)
        {
            // Las validaciones ahora son más específicas.
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del platillo no puede estar vacío.", nameof(nombre));
            }

            if (string.IsNullOrWhiteSpace(categoria))
            {
                throw new ArgumentException("La categoria del platillo no puede estar vacío.", nameof(categoria));
            }

            if (precio < 0)
            {
                throw new ArgumentException("El precio no puede ser un valor negativo.", nameof(precio));
            }

            Platillo nuevo = new()
            {
                Nombre = nombre,
                Categoria = categoria,
                PrecioVenta = precio
            };

            // Llamamos al método asíncrono
            await _repositorio.AgregarAsync(nuevo);
        }

        /// <summary>
        /// Elimina un platillo de forma asíncrona.
        /// </summary>
        public async Task EliminarPlatilloAsync(int id)
        {
            await _repositorio.EliminarAsync(id);
        }
    }
}