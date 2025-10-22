using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
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
        Task<List<Producto>> ObtenerTodosAsync();        // R - Read (Leer todos)
        Task<Producto> ObtenerPorIdAsync(int id);      // R - Read (Leer uno)
        Task AgregarAsync(Producto producto);      // C - Create (Crear)
        Task ActualizarAsync(Producto producto);   // U - Update (Actualizar)
        Task EliminarAsync(int id);                // D - Delete (Eliminar)
        Task ActualizarStockAsync(int idProducto, decimal cantidadADescontar, SqlConnection connection, SqlTransaction transaction);    // Actualiza el stock de un producto específico
    }
}
