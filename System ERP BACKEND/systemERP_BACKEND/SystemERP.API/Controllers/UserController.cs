using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Security;

namespace SystemERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var users = await _userService.GetAll();
                return Ok(new Response<List<UserDTO>> { status = true, value = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<UserDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _userService.GetById(id);
                if (user == null)
                {
                    return NotFound(new Response<UserDTO> { status = false, msg = "Usuario no encontrado." });
                }
                return Ok(new Response<UserDTO> { status = true, value = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<UserDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var createdUser = await _userService.Register(registerDto);
                return Ok(new Response<UserDTO> { status = true, value = createdUser });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new Response<UserDTO> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<UserDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("UpdateName/{id}")]
        public async Task<IActionResult> UpdateName(Guid id, [FromBody] UpdateUserNameDTO dto)
        {
            try
            {
                var result = await _userService.UpdateName(id, dto);
                return Ok(new Response<bool> { status = result, value = result, msg = result ? "Nombre actualizado correctamente." : "No se pudo actualizar." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<bool> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("UpdateInfo/{id}")]
        public async Task<IActionResult> UpdateInfo(Guid id, [FromBody] UpdateUserInfoDTO dto)
        {
            try
            {
                var result = await _userService.UpdateInfo(id, dto);
                return Ok(new Response<bool> { status = result, value = result, msg = result ? "Información actualizada correctamente." : "No se pudo actualizar." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<bool> { status = false, msg = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new Response<bool> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("UpdateRole/{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateUserRoleDTO dto)
        {
            try
            {
                var result = await _userService.UpdateRole(id, dto);
                return Ok(new Response<bool> { status = result, value = result, msg = result ? "Rol actualizado correctamente." : "No se pudo actualizar." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<bool> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var result = await _userService.ToggleStatus(id);
                return Ok(new Response<bool> { status = result, value = result, msg = result ? "Estado de usuario actualizado correctamente." : "No se pudo actualizar." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<bool> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }
    }
}
