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
    public class MenuController
    {
        // Cambiamos el tipo de repositorio
        private readonly IPlatilloRepository _repositorio;

        public MenuController()
        {
            _repositorio = new PlatilloMockRepository();
        }

        public List<Platillo> CargarPlatillos()
        {
            return _repositorio.ObtenerTodos();
        }

        public void GuardarNuevoPlatillo(string nombre, string categoria, decimal precio)
        {
            // ¡Usamos la validación del controlador!
            if (string.IsNullOrEmpty(nombre) || precio <= 0)
            {
                throw new System.Exception("Datos del platillo inválidos.");
            }

            Platillo nuevo = new Platillo
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