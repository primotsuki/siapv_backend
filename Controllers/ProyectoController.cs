using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Models;
using siapv_backend.Services;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectoController: ControllerBase
    {
        private readonly IProyectoService _proyectoService;
        public ProyectoController(IProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
        }
        [Authorize]
        [HttpPost("proyecto")]
        public async Task<IActionResult> createProyecto([FromBody] Proyecto feriado)
        {
            var result = await _proyectoService.createProyectos(feriado.descripcion);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("proyectos")]
        public async Task<IActionResult> getFeriados()
        {
            var result = await _proyectoService.getProyectosList();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}