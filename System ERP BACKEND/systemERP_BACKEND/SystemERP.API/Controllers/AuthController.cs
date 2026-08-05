using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Security;

namespace SystemERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var session = await _authService.Login(loginDto);
                return Ok(new Response<SessionDTO> { status = true, value = session });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new Response<SessionDTO> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SessionDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var session = await _authService.Register(registerDto);
                return Ok(new Response<SessionDTO> { status = true, value = session });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new Response<SessionDTO> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SessionDTO> { status = false, msg = ex.Message });
            }
        }
    }
}
