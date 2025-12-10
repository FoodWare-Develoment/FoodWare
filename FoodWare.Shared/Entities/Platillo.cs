using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Shared.Entities
{
  public class Platillo
    {
        public int IdPlatillo { get; set; }
        public required string Nombre { get; set; }
        public required string Categoria { get; set; } // Ej: "Bebidas", "Postres"
        public decimal PrecioVenta { get; set; }
    }

}
