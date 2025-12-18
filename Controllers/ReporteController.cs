using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
using siapv_backend.Services;
using Microsoft.AspNetCore.Authorization;
namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController: ControllerBase
    {
        private readonly IReportService _reportService;
        public ReporteController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [Authorize]
        [HttpGet("memorandum")]
        public async Task<IActionResult> getMemorandum(int id)
        {
            var pdfBytes = await _reportService.getMemorandumDoc(id);
            return File(pdfBytes, "application/pdf", "document.pdf");
        }
        [Authorize]
        [HttpGet("fucav")]
        public async Task<IActionResult> getFucav(int id)
        {
            var pdfBytes = await _reportService.getFucavDoc(id);
            return File(pdfBytes, "application/pdf", "documment.pdf");
        }
        [Authorize]
        [HttpGet("certificacion-presupuestaria")]
        public async Task<IActionResult> getCertificacionPresupuestaria(int id)
        {
            var pdfBytes = await _reportService.getCertificacionPresupuestaria(id);
            return File(pdfBytes, "application/pdf", "document.pdf");
        }
        [Authorize]
        [HttpGet("certificacion-poa")]
        public async Task<IActionResult> getCertificacionPoa(int id)
        {
            var pdfBytes = await _reportService.getCertificacionPOA(id);
            return File(pdfBytes, "application/pdf", "document.pdf");
        }
    }
}