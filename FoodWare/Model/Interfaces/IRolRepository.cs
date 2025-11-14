using FoodWare.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Model.Interfaces
{
    public interface IRolRepository
    {
        Task<List<Rol>> ObtenerTodosAsync();
    }
}