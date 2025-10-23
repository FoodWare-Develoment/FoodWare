using System;
using System.Collections.Generic;
// <<<<<<< HEAD

namespace FoodWare.Model.Entities
{
    // 6. TABLA: RECETAS
    public class Receta
    {
        public int IdReceta { get; set; }
        public int IdItem { get; set; } // FOREIGN KEY, NOT NULL
        public int IdProducto { get; set; } // FOREIGN KEY, NOT NULL
        public decimal CantidadRequerida { get; set; } // NOT NULL
    }
}
// ======= 
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
// >>>>>>> master
