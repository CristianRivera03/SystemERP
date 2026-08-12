using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.API.Utility;
using SystemERP.BLL.Services.Contract;
using SystemERP.DTO.Inventory;

namespace SystemERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = new Response<List<WarehouseCategoryDTO>>();
            try
            {
                response.status = true;
                response.value = await _warehouseService.GetWarehouseCategoriesAsync();
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
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var response = new Response<List<WarehouseDTO>>();
            try
            {
                response.status = true;
                response.value = await _warehouseService.GetAllWarehousesAsync();
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
            var response = new Response<WarehouseDTO>();
            try
            {
                var item = await _warehouseService.GetWarehouseByIdAsync(id);
                if (item == null)
                {
                    response.status = false;
                    response.msg = "Almacén no encontrado";
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
        public async Task<IActionResult> Create([FromBody] WarehouseDTO dto)
        {
            var response = new Response<WarehouseDTO>();
            try
            {
                response.status = true;
                response.value = await _warehouseService.CreateWarehouseAsync(dto);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] WarehouseDTO dto)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _warehouseService.UpdateWarehouseAsync(id, dto);
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
                response.value = await _warehouseService.ToggleWarehouseStatusAsync(id);
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
        [Route("{warehouseId:guid}/Locations")]
        public async Task<IActionResult> GetLocations(Guid warehouseId)
        {
            var response = new Response<List<LocationDTO>>();
            try
            {
                response.status = true;
                response.value = await _warehouseService.GetLocationsByWarehouseAsync(warehouseId);
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
        [Route("CreateLocation")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDTO dto)
        {
            var response = new Response<LocationDTO>();
            try
            {
                response.status = true;
                response.value = await _warehouseService.CreateLocationAsync(dto);
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
