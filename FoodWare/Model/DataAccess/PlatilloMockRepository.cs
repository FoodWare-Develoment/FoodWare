using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;

namespace FoodWare.Model.DataAccess
{
    public class PlatilloMockRepository : IPlatilloRepository
    {
        private static List<Platillo> _platillos = new()
        {
            new() { IdPlatillo = 1, Nombre = "Hamburguesa Clásica", Categoria = "Platos Fuertes", PrecioVenta = 120m },
            new() { IdPlatillo = 2, Nombre = "Refresco de Cola", Categoria = "Bebidas", PrecioVenta = 35m },
            new() { IdPlatillo = 3, Nombre = "Pastel de Chocolate", Categoria = "Postres", PrecioVenta = 75m }
        };
        private static int _nextId = 4;

        public List<Platillo> ObtenerTodos() => _platillos.ToList();

        public void Agregar(Platillo platillo)
        {
            platillo.IdPlatillo = _nextId++;
            _platillos.Add(platillo);
        }

        public void Eliminar(int id)
        {
            var platillo = _platillos.FirstOrDefault(p => p.IdPlatillo == id);
            if (platillo != null) _platillos.Remove(platillo);
        }

        // (Puedes dejar Actualizar y ObtenerPorId para después)
        public Platillo ObtenerPorId(int id) => throw new NotImplementedException();
        public void Actualizar(Platillo platillo) => throw new NotImplementedException();
    }
}