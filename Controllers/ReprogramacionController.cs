using Microsoft.AspNetCore.Mvc;
using siapv_backend.Models;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
using siapv_backend.Services;
using siapv_backend.Models.DTORequests;

namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReprogramacionController: ControllerBase
    {
        private readonly IReprogramacionService _reprogService;
        public ReprogramacionController(IReprogramacionService repSer)
        {
            _reprogService = repSer;
        }
        [Authorize]
        [HttpPost("reprogramacion")]
        public async Task<IActionResult> createReprogramacion([FromBody] DTOReprogramacion request)
        {
            var reprog = await _reprogService.createReprogramacion(request);
            if(reprog == null)
            {
                return BadRequest();
            }
            return Ok(reprog);
        }
        [Authorize]
        [HttpGet("reprogramacion-reporte")]
        public async Task<IActionResult> getRepogramacionReporte(int id)
        {
            var pdfBytes = await _reprogService.getFormularioReprogramacion(id);
            return File(pdfBytes, "application/pdf", "document.pdf");
        }
    }
}