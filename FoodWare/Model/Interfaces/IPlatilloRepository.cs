using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Model.Entities;

namespace FoodWare.Model.Interfaces
{
    /// <summary>
    /// Define las operaciones CRUD para la entidad Platillo, 
    /// proporcionando los métodos necesarios para gestionar productos en el sistema.
    /// </summary>
    public interface IPlatilloRepository
    {
        List<Platillo> ObtenerTodos();              // R - Read (Leer todos)
        Platillo ObtenerPorId(int id);              // R - Read (Leer uno)
        void Agregar(Platillo platillo);            // C - Create (Crear)
        void Actualizar(Platillo platillo);         // U - Update (Actualizar)
        void Eliminar(int id);                      // D - Delete (Eliminar)
    }
}
