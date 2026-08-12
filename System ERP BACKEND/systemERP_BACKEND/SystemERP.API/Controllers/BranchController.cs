using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Entities;

namespace SystemERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var response = new Response<List<BranchDTO>>();
            try
            {
                response.status = true;
                response.value = await _branchService.GetAllBranchesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = new Response<BranchDTO>();
            try
            {
                var item = await _branchService.GetBranchByIdAsync(id);
                if (item == null)
                {
                    response.status = false;
                    response.msg = "Sucursal no encontrada";
                    return NotFound(response);
                }
                response.status = true;
                response.value = item;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] BranchDTO dto)
        {
            var response = new Response<BranchDTO>();
            try
            {
                response.status = true;
                response.value = await _branchService.CreateBranchAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        [Route("Update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BranchDTO dto)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _branchService.UpdateBranchAsync(id, dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPatch]
        [Route("ToggleStatus/{id:guid}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _branchService.ToggleBranchStatusAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
