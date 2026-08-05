using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Security;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class ActionLogService : IActionLogService
    {
        private readonly IGenericRepository<ActionLog> _logRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ActionLogService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionLogService(
            IGenericRepository<ActionLog> logRepository,
            IMapper mapper,
            ILogger<ActionLogService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logRepository = logRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(Guid? userId, string action, string affectedTable, string recordId, string? details, string? ipAddress = null)
        {
            try
            {
                // Auto-resolve authenticated user performing the action if not explicitly supplied
                if (!userId.HasValue || userId.Value == Guid.Empty)
                {
                    var userClaims = _httpContextAccessor.HttpContext?.User;
                    var claimId = userClaims?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                        ?? userClaims?.FindFirst("sub")?.Value;

                    if (!string.IsNullOrEmpty(claimId) && Guid.TryParse(claimId, out var currentUserId))
                    {
                        userId = currentUserId;
                    }
                }

                // Auto-resolve remote client IP address if not supplied
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                }

                IPAddress? parsedIp = null;
                if (!string.IsNullOrEmpty(ipAddress) && IPAddress.TryParse(ipAddress, out var ip))
                {
                    parsedIp = ip;
                }

                // PostgreSQL jsonb column requires valid JSON string
                string jsonDetails;
                if (string.IsNullOrWhiteSpace(details))
                {
                    jsonDetails = "{}";
                }
                else
                {
                    var trimmed = details.Trim();
                    if ((trimmed.StartsWith("{") && trimmed.EndsWith("}")) || (trimmed.StartsWith("[") && trimmed.EndsWith("]")))
                    {
                        jsonDetails = trimmed;
                    }
                    else
                    {
                        jsonDetails = JsonSerializer.Serialize(new { description = details });
                    }
                }

                var log = new ActionLog
                {
                    IdLog = Guid.NewGuid(),
                    IdUser = userId,
                    Action = action,
                    AffectedTable = affectedTable,
                    RecordId = recordId,
                    Details = jsonDetails,
                    SourceIp = parsedIp,
                    ActionDate = DateTime.UtcNow
                };

                await _logRepository.Create(log);
            }
            catch (Exception ex)
            {
                // Action logging failure should never crash the main operation
                _logger.LogError(ex, "Error al registrar log de auditoría para la acción {Action} en tabla {Table}", action, affectedTable);
            }
        }

        public async Task<List<ActionLogDTO>> GetAllLogsAsync()
        {
            try
            {
                var query = _logRepository.Query();
                var logs = await query
                    .Include(l => l.IdUserNavigation)
                    .OrderByDescending(l => l.ActionDate)
                    .ToListAsync();

                var dtoList = _mapper.Map<List<ActionLogDTO>>(logs);
                foreach (var dto in dtoList)
                {
                    dto.Details = ExtractDetailsText(dto.Details);
                }

                return dtoList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la bitácora de auditoría.");
                throw;
            }
        }

        public async Task<List<ActionLogDTO>> GetLogsByTableAsync(string tableName)
        {
            try
            {
                var query = _logRepository.Query(l => l.AffectedTable.ToLower() == tableName.ToLower());
                var logs = await query
                    .Include(l => l.IdUserNavigation)
                    .OrderByDescending(l => l.ActionDate)
                    .ToListAsync();

                var dtoList = _mapper.Map<List<ActionLogDTO>>(logs);
                foreach (var dto in dtoList)
                {
                    dto.Details = ExtractDetailsText(dto.Details);
                }

                return dtoList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la bitácora de la tabla {Table}", tableName);
                throw;
            }
        }

        private static string ExtractDetailsText(string? jsonbDetails)
        {
            if (string.IsNullOrWhiteSpace(jsonbDetails)) return string.Empty;
            try
            {
                using var doc = JsonDocument.Parse(jsonbDetails);
                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("description", out var descElement))
                {
                    return descElement.GetString() ?? jsonbDetails;
                }
            }
            catch
            {
                // Return original string if not JSON object
            }
            return jsonbDetails;
        }
    }
}
