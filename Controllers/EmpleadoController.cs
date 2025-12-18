using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Models.DTOResponses;
using AuthorizeAttribute = siapv_backend.Helpers.AuthorizeAttribute;
using siapv_backend.Services;
using siapv_backend.Models.DTORequests;
namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController: ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;
        public EmpleadoController (IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }
        [Authorize]
        [HttpGet("empleados-dependencia/{id}")]
        public async Task<IActionResult> getEmpleadoByDependencia(int id){
            var result = await _empleadoService.getempleadosByDependencia(id);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> getUsersById([FromQuery] DTOUserParams request)
        {
            var users = await _empleadoService.getActiveUsers(request);
            if(users == null)
            {
                return BadRequest();
            }
            return Ok(users);
        }
        [Authorize]
        [HttpGet("empleados/roles")]
        public async Task<IActionResult> getRoles()
        {
            var roles = await _empleadoService.getRoles();
            if(roles == null)
            {
                return BadRequest();
            }
            return Ok(roles);
        }
        [Authorize]
        [HttpPost("empleado/role")]
        public async Task<IActionResult> changeRole([FromBody] DTOUserRole request)
        {
            var userRole = await _empleadoService.updateUserRole(request);
            if(userRole == null)
            {
                return BadRequest();
            }
            return Ok(userRole);
        }
    }
}