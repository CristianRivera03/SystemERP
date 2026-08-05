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
    public class ActionLogController : ControllerBase
    {
        private readonly IActionLogService _actionLogService;

        public ActionLogController(IActionLogService actionLogService)
        {
            _actionLogService = actionLogService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var logs = await _actionLogService.GetAllLogsAsync();
                return Ok(new Response<List<ActionLogDTO>> { status = true, value = logs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<ActionLogDTO>> { status = false, msg = ex.Message });
            }
        }

        [HttpGet("Table/{tableName}")]
        public async Task<IActionResult> GetByTable(string tableName)
        {
            try
            {
                var logs = await _actionLogService.GetLogsByTableAsync(tableName);
                return Ok(new Response<List<ActionLogDTO>> { status = true, value = logs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<List<ActionLogDTO>> { status = false, msg = ex.Message });
            }
        }
    }
}
