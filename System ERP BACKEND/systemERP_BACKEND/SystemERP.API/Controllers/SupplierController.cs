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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] string? search = null)
        {
            try
            {
                var list = await _supplierService.GetSuppliersAsync(search);
                return Ok(new Response<List<SupplierDTO>> { status = true, value = list });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<SupplierDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _supplierService.GetSupplierByIdAsync(id);
                if (item == null)
                {
                    return NotFound(new Response<SupplierDTO> { status = false, msg = "Proveedor no encontrado" });
                }
                return Ok(new Response<SupplierDTO> { status = true, value = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SupplierDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SupplierDTO dto)
        {
            try
            {
                var created = await _supplierService.CreateSupplierAsync(dto);
                return Ok(new Response<SupplierDTO> { status = true, value = created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SupplierDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SupplierDTO dto)
        {
            try
            {
                dto.IdSupplier = id;
                var updated = await _supplierService.UpdateSupplierAsync(dto);
                return Ok(new Response<SupplierDTO> { status = true, value = updated });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SupplierDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpPatch("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var result = await _supplierService.ToggleStatusAsync(id);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }

        [HttpPost("Contact")]
        public async Task<IActionResult> AddContact([FromBody] SupplierContactDTO dto)
        {
            try
            {
                var contact = await _supplierService.AddContactAsync(dto);
                return Ok(new Response<SupplierContactDTO> { status = true, value = contact });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<SupplierContactDTO> { status = false, msg = ex.Message });
            }
        }

        [HttpDelete("Contact/{contactId}")]
        public async Task<IActionResult> DeleteContact(int contactId)
        {
            try
            {
                var result = await _supplierService.DeleteContactAsync(contactId);
                return Ok(new Response<bool> { status = true, value = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool> { status = false, msg = ex.Message });
            }
        }
    }
}
