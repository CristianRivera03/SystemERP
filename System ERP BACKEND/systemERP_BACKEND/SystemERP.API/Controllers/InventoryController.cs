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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        [Route("Stock")]
        public async Task<IActionResult> GetStock([FromQuery] Guid? branchId, [FromQuery] Guid? warehouseId)
        {
            var response = new Response<List<InventoryStockDTO>>();
            try
            {
                response.status = true;
                response.value = await _inventoryService.GetStocksAsync(branchId, warehouseId);
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
        [Route("AdjustStock/{idStock:guid}")]
        public async Task<IActionResult> AdjustStock(Guid idStock, [FromQuery] decimal newQuantity, [FromQuery] string? reason)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _inventoryService.AdjustStockAsync(idStock, newQuantity, reason);
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
