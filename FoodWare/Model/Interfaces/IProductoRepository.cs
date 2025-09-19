using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Model.Entities;

namespace FoodWare.Model.Interfaces
{
    /// <summary>
    /// Define las operaciones CRUD para la entidad Producto, 
    /// proporcionando los métodos necesarios para gestionar productos en el sistema.
    /// </summary>
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();        // R - Read (Leer todos)
        Producto ObtenerPorId(int id);      // R - Read (Leer uno)
        void Agregar(Producto producto);      // C - Create (Crear)
        void Actualizar(Producto producto);   // U - Update (Actualizar)
        void Eliminar(int id);                // D - Delete (Eliminar)
    }
}
