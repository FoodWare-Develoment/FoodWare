using FoodWare.Model.Entities;
using FoodWare.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodWare.Controller.Logic
{
    public class RecetaController(IRecetaRepository repositorio)
    {
        private readonly IRecetaRepository _repositorio = repositorio;

        public async Task<List<RecetaDetalle>> CargarRecetaDePlatilloAsync(int idPlatillo)
        {
            return await _repositorio.ObtenerPorPlatilloAsync(idPlatillo);
        }

        public async Task GuardarNuevoIngredienteAsync(int idPlatillo, int idProducto, decimal cantidad)
        {
            if (idPlatillo <= 0 || idProducto <= 0)
            {
                throw new ArgumentException("Debe seleccionar un platillo y un producto.");
            }
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
            }

            Receta nuevoIngrediente = new()
            {
                IdPlatillo = idPlatillo,
                IdProducto = idProducto,
                Cantidad = cantidad
            };

            await _repositorio.AgregarAsync(nuevoIngrediente);
        }

        public async Task EliminarIngredienteAsync(int idReceta)
        {
            if (idReceta <= 0)
            {
                throw new ArgumentException("Debe seleccionar un ingrediente válido para eliminar.");
            }
            await _repositorio.EliminarAsync(idReceta);
        }
    }
}