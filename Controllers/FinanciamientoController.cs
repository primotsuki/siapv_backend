using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Models;
using siapv_backend.Services;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinanciamientoController: ControllerBase
    {
        private readonly IFinanciamientoService _financiamientoService;
        public FinanciamientoController(IFinanciamientoService financiamientoService)
        {
            _financiamientoService = financiamientoService;
        }
        [Authorize]
        [HttpGet("financiamiento")]
        public async Task<IActionResult> getFeriados()
        {
            var result = await _financiamientoService.getFinanciamientoList();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}