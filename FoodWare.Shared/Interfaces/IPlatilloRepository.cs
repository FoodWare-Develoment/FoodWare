using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Shared.Entities;

namespace FoodWare.Shared.Interfaces
{
    /// <summary>
    /// Define las operaciones CRUD para la entidad Platillo, 
    /// proporcionando los métodos necesarios para gestionar productos en el sistema.
    /// </summary>
    public interface IPlatilloRepository
    {
        Task<List<Platillo>> ObtenerTodosAsync();              // R - Read (Leer todos)
        Task<Platillo> ObtenerPorIdAsync(int id);              // R - Read (Leer uno)
        Task AgregarAsync(Platillo platillo);            // C - Create (Crear)
        Task ActualizarAsync(Platillo platillo);         // U - Update (Actualizar)
        Task EliminarAsync(int id);                      // D - Delete (Eliminar)
    }
}
