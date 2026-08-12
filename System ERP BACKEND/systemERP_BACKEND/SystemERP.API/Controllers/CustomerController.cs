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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] string? search = null)
        {
            try
            {
                var list = await _customerService.GetCustomersAsync(search);
                return Ok(new Response<List<CustomerDTO>> { status = true, value = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<CustomerDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _customerService.GetCustomerByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new Response<CustomerDTO> { status = false, msg = "Cliente no encontrado" });
                }
                return Ok(new Response<CustomerDTO> { status = true, value = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CustomerDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CustomerDTO dto)
        {
            try
            {
                var created = await _customerService.CreateCustomerAsync(dto);
                return Ok(new Response<CustomerDTO> { status = true, value = created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CustomerDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerDTO dto)
        {
            try
            {
                dto.IdCustomer = id;
                var updated = await _customerService.UpdateCustomerAsync(dto);
                return Ok(new Response<CustomerDTO> { status = true, value = updated });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<CustomerDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPatch("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var result = await _customerService.ToggleStatusAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }
    }
}
