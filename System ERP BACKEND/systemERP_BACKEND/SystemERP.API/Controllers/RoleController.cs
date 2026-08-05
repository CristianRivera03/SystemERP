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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var result = await _roleService.GetRolesWithModulesAsync();
                return Ok(new Response<List<RoleWithModulesDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<RoleWithModulesDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Modules")]
        public async Task<IActionResult> GetModules()
        {
            try
            {
                var result = await _roleService.GetAllModulesAsync();
                return Ok(new Response<List<ModuleDTO>> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<ModuleDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                var result = await _roleService.CreateRoleAsync(request.RoleName, request.ModuleIds);
                return Ok(new Response<RoleWithModulesDTO> { status = true, value = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new Response<RoleWithModulesDTO> { status = false, msg = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<RoleWithModulesDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("Permissions")]
        public async Task<IActionResult> UpdatePermissions([FromBody] UpdateRolePermissionsDTO dto)
        {
            try
            {
                var result = await _roleService.UpdateRolePermissionsAsync(dto);
                return Ok(new Response<bool> { status = true, value = result });
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }
    }

    public class CreateRoleRequest
    {
        public string RoleName { get; set; } = null!;
        public List<int>? ModuleIds { get; set; }
    }
}
