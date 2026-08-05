using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Security;

namespace SystemERP.BLL.Services.Contract
{
    public interface IActionLogService
    {
        Task LogActionAsync(Guid? userId, string action, string affectedTable, string recordId, string? details, string? ipAddress = null);
        Task<List<ActionLogDTO>> GetAllLogsAsync();
        Task<List<ActionLogDTO>> GetLogsByTableAsync(string tableName);
    }
}
