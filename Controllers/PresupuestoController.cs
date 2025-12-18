using siapv_backend.Models.DTORequests;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Services;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;

namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController: ControllerBase
    {
        private readonly IPresupuestoService _presupuestoService;
        public PresupuestoController(IPresupuestoService presupuestoService)
        {
            _presupuestoService = presupuestoService;
        }
        [Authorize]
        [HttpGet("categorias-progs")]
        public async Task<IActionResult> getCategoriasProg()
        {
            var result = await _presupuestoService.getCategorias();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("certificacion-presupuestaria")]
        public async Task<IActionResult> createCertificacion([FromBody] DTOCertificacionPres request)
        {
            var result = await _presupuestoService.createCertificacion(request);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}