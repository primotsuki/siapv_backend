using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Models.DTORequests;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
using siapv_backend.Services;
using siapv_backend.Models.DTOResponses;
using siapv_backend.Models;
using FluentEmail.Core;
namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudViajeController: ControllerBase
    {
       private readonly ISolicitudViajeService _solicitudService;
       private readonly IEmpleadoService _empleadoService;
       private readonly IEmailService _emailService;
       public SolicitudViajeController (ISolicitudViajeService solService, IEmpleadoService empleadoService, IEmailService emailService)
        {
            _solicitudService = solService;
            _empleadoService = empleadoService;
            _emailService = emailService;
        }
        [Authorize]
        [HttpGet("solicitudes-dependencia")]
        public async Task<IActionResult> getSolicitudesByDependencia()
        {
            HttpContext.Items.TryGetValue("User", out var usuario);
            DTOUserContext? userContext = usuario as DTOUserContext;
            var empleado = await _empleadoService.getEmpleadoActivoByPersonaId(userContext?.personaId);
            var result = await _solicitudService.getSolicitudesByDependencia(empleado.DependenciaId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("solicitudes-estado")]
        public async Task<IActionResult> getSolicitudesByEstado([FromQuery] DTOsolicitudReq request)
        {
            var result = await _solicitudService.getSolicitudByEstados(request);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("generar-memorandum")]
        public async Task<IActionResult> generarMemorandum([FromBody] DTOMemorandum memo)
        {
            // Obtener el empleadoId del token o contexto de usuario autenticado
            HttpContext.Items.TryGetValue("User", out var usuario);
            DTOUserContext? userContext = usuario as DTOUserContext;
            var empleado = await _empleadoService.getEmpleadoActivoByPersonaId(userContext?.personaId);
            var result = await _solicitudService.generarMemorandum(memo, empleado.Id);
            var sent = await _emailService.sendInicioViaje(result);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("destinos")]
        public async Task<IActionResult> GetDestinos()
        {
            var result = await _solicitudService.GetDestinos();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("tipos-viaje")]
        public async Task<IActionResult> getTiposViaje()
        {
            var result = await _solicitudService.getTiposViaje();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("mis-solicitudes")]
        public async Task<IActionResult> getMisSolicitudes()
        {
            // Obtener el empleadoId del token o contexto de usuario autenticado
            HttpContext.Items.TryGetValue("User", out var usuario);
            DTOUserContext? userContext = usuario as DTOUserContext;
            var empleado = await _empleadoService.getEmpleadoActivoByPersonaId(userContext?.personaId);
            var result = await _solicitudService.getSolicitudes(empleado != null ? empleado.Id : 0);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("mis-pendientes")]
        public async Task<IActionResult> misPendientes()
        {
            HttpContext.Items.TryGetValue("User", out var usuario);
            DTOUserContext? userContext = usuario as DTOUserContext;
            var empleado = await _empleadoService.getEmpleadoActivoByPersonaId(userContext?.personaId);
            var result = await _solicitudService.getPendientes(empleado != null ? empleado.Id : 0);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("solicitud/{id}")]
        public async Task<IActionResult> getSolicitudById(int id)
        {
            var result = await _solicitudService.getSolicitudById(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("completar-solicitud")]
        public async Task<IActionResult> completarSolicitud([FromBody] DTOFucav solicitud)
        {
            var result = await _solicitudService.completarSolicitud(solicitud);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("medios-transporte")]
        public async Task<IActionResult> getMediosTransporte()
        {
            var result = await _solicitudService.getMediosTransporte();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("solicitudes-search")]
        public async Task<IActionResult> getSolicitudesSearch([FromQuery] DTOsolParams request)
        {
            var result = await _solicitudService.getSolicitudesbyParams(request);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("revision-solicitud")]
        public async Task<IActionResult> createRevisionFormulario([FromBody] DTORevision revision)
        {
            var result = await _solicitudService.crearRevisiondeFormulario(revision);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("estados")]
        public async Task<IActionResult> getEstados()
        {
            var result = await _solicitudService.getEstados();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}