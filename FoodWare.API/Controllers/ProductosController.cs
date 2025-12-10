using Microsoft.AspNetCore.Mvc;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;

namespace FoodWare.API.Controllers
{
    [Route("api/[controller]")] // La ruta será: api/productos
    [ApiController]
    public class ProductosController(IProductoRepository repositorio) : ControllerBase
    {
        private readonly IProductoRepository _repositorio = repositorio;

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> ObtenerTodos()
        {
            try
            {
                var productos = await _repositorio.ObtenerTodosAsync();
                return Ok(productos); // Retorna código 200 OK con la lista JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> ObtenerPorId(int id)
        {
            var producto = await _repositorio.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound("Producto no encontrado");
            return Ok(producto);
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] Producto nuevoProducto)
        {
            if (nuevoProducto == null) return BadRequest();

            // Aquí podrías agregar validaciones extra si quisieras
            await _repositorio.AgregarAsync(nuevoProducto);
            return Ok("Producto creado exitosamente");
        }

        // PUT: api/productos
        [HttpPut]
        public async Task<ActionResult> Actualizar([FromBody] Producto producto)
        {
            if (producto == null || producto.IdProducto <= 0) return BadRequest();

            await _repositorio.ActualizarAsync(producto);
            return Ok("Producto actualizado");
        }
    }
}