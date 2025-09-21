using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;

namespace FoodWare.Model.DataAccess
{
    // Enlace con IProductoRepository
    public class ProductoMockRepository : IProductoRepository
    {
        // Nuestra "base de datos" falsa
        private static List<Producto> _productos = new()
        {
            new() { IdProducto = 1, Nombre = "Tomate", Categoria = "Verduras", StockActual = 50, PrecioCosto = 20.5m },
            new() { IdProducto = 2, Nombre = "Pechuga de Pollo", Categoria = "Carnes", StockActual = 30, PrecioCosto = 80m },
            new() { IdProducto = 3, Nombre = "Pan de Hamburguesa", Categoria = "Panadería", StockActual = 100, PrecioCosto = 5m }
        };
        private static int _nextId = 4; // Para simular el auto-incremento

        public List<Producto> ObtenerTodos()
        {
            // Devuelve una copia para simular que vienen de la BD
            return _productos.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            // Si no se encuentra, lanza una excepción para cumplir con la interfaz
            var producto = _productos.FirstOrDefault(p => p.IdProducto == id);
            return producto ?? throw new InvalidOperationException($"No se encontró el producto con Id {id}.");
        }

        public void Agregar(Producto producto)
        {
            producto.IdProducto = _nextId++;
            _productos.Add(producto);
        }

        public void Actualizar(Producto producto)
        {
            var existente = _productos.FirstOrDefault(p => p.IdProducto == producto.IdProducto);
            if (existente != null)
            {
                existente.Nombre = producto.Nombre;
                existente.Categoria = producto.Categoria;
                existente.StockActual = producto.StockActual;
                existente.PrecioCosto = producto.PrecioCosto;
            }
        }

        public void Eliminar(int id)
        {
            var producto = _productos.FirstOrDefault(p => p.IdProducto == id);
            if (producto != null)
            {
                _productos.Remove(producto);
            }
        }
    }
}