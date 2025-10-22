using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Model.Entities
{
    public class Receta
    {
        public int IdReceta { get; set; }
        public int IdPlatillo { get; set; }
        public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
    }
}