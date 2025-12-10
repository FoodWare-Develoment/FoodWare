using FoodWare.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Shared.Interfaces
{
    public interface IRolRepository
    {
        Task<List<Rol>> ObtenerTodosAsync();
    }
}