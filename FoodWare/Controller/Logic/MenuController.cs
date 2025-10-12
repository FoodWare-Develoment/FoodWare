using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using FoodWare.Model.DataAccess; // Importante

namespace FoodWare.Controller.Logic
{
    public class MenuController(IPlatilloRepository repositorio)
    {
        // solo conozca la INTERFAZ (la idea de un repositorio).
        private readonly IPlatilloRepository _repositorio = repositorio;

        public List<Platillo> CargarPlatillos()
        {
            return _repositorio.ObtenerTodos();
        }

        /// <summary>
        /// Guarda un nuevo platillo en el repositorio.
        /// </summary>
        /// <param name="nombre">Nombre del producto. No puede ser nulo o vacío.</param>
        /// <param name="categoria">Categoría del producto.</param>
        /// <param name="precio">Precio de costo del platillo. No puede ser negativo.</param>
        /// <exception cref="ArgumentException">Se lanza si el nombre o categoria está vacío o si el precio son negativos.</exception>
        public void GuardarNuevoPlatillo(string nombre, string categoria, decimal precio)
        {
            // Las validaciones ahora son más específicas.
            if (string.IsNullOrWhiteSpace(nombre))
            {
                // Esta excepción indica que un argumento (parámetro) tiene un valor inválido.
                throw new ArgumentException("El nombre del platillo no puede estar vacío.", nameof(nombre));
            }

            if (string.IsNullOrWhiteSpace(categoria))
            {
                // Esta excepción indica que un argumento (parámetro) tiene un valor inválido.
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
            _repositorio.Agregar(nuevo);
        }

        public void EliminarPlatillo(int id)
        {
            _repositorio.Eliminar(id);
        }
    }
}