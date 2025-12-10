using Microsoft.AspNetCore.Mvc;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;

namespace FoodWare.API.Controllers
{
    [Route("api/[controller]")] // La ruta será: /api/platillos
    [ApiController]
    public class PlatillosController(IPlatilloRepository repositorio) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Platillo>>> ObtenerTodos()
        {
            try
            {
                var platillos = await repositorio.ObtenerTodosAsync();
                return Ok(platillos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}