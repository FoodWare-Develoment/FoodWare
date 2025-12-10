using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWare.Shared.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public required string Nombre { get; set; }
        public required string Categoria { get; set; }
        public required string UnidadMedida { get; set; }
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public decimal PrecioCosto { get; set; }
    }
}