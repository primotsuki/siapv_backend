using siapv_backend.Models.DTORequests;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace siapv_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LdapRequest request)
        {
            var authenticatedUser = await _userService.Authenticate(request);
            if (authenticatedUser != null)
            {
                return Ok(authenticatedUser);
            }
            return BadRequest(new { Message = "usuario o contraseña incorrectos" });
        }
    }
}