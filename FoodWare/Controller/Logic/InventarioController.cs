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
    public class InventarioController
    {
        // El controlador SÓLO conoce la interfaz
        private readonly ProductoMockRepository _repositorio;

        public InventarioController()
        {
            // Le decimos que use el Repositorio FALSO.
            _repositorio = new ProductoMockRepository();

            // Cuando tengamos la BD, solo tendremos que camiar esta linea:
            // _repositorio = new ProductoSqlRepository(); 
        }

        // El controlador expone los métodos que la Vista necesita
        public List<Producto> CargarProductos()
        {
            return _repositorio.ObtenerTodos();
        }

        public void GuardarNuevoProducto(string nombre, string categoria, int stock, decimal precio)
        {
            // Aquí podríamos agregar validaciones
            if (string.IsNullOrEmpty(nombre) || stock < 0 || precio < 0)
            {
                throw new System.Exception("Datos del producto inválidos.");
            }

            Producto nuevo = new Producto
            {
                Nombre = nombre,
                Categoria = categoria,
                StockActual = stock,
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