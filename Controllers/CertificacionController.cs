using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
using siapv_backend.Services;
using siapv_backend.Models.DTORequests;

namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificacionController: ControllerBase
    {
        private readonly ICertificacionPOAService _certificacionService;
        public CertificacionController(ICertificacionPOAService certService)
        {
            _certificacionService = certService;
        }
        [Authorize]
        [HttpGet("actividades-poa")]
        public async Task<IActionResult> getActividadesPOA([FromQuery] string tipo)
        {
            var result = await _certificacionService.getActividadesPOA(tipo);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("operaciones-poa")]
        public async Task<IActionResult> getOperacionesPOA()
        {
            var result = await _certificacionService.getOperacionesPOA();
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("certificacion-poa")]
        public async Task<IActionResult> createCertificacion([FromBody] DTOCertificacionPOA request)
        {
            var result = await _certificacionService.createCertificacionPOA(request);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}