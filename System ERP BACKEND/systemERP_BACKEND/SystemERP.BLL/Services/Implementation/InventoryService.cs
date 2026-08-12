using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemERP.BLL.Services.Contract;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DTO.Inventory;
using SystemERP.Model;

namespace SystemERP.BLL.Services.Implementation
{
    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository<InventoryStock> _stockRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryService> _logger;
        private readonly IActionLogService _actionLogService;

        public InventoryService(
            IGenericRepository<InventoryStock> stockRepository,
            IMapper mapper,
            ILogger<InventoryService> logger,
            IActionLogService actionLogService)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
            _logger = logger;
            _actionLogService = actionLogService;
        }

        public async Task<List<InventoryStockDTO>> GetStocksAsync(Guid? branchId = null, Guid? warehouseId = null)
        {
            try
            {
                var query = _stockRepository.Query();

                if (warehouseId.HasValue && warehouseId.Value != Guid.Empty)
                {
                    query = query.Where(s => s.IdLocationNavigation.IdWarehouse == warehouseId.Value);
                }
                else if (branchId.HasValue && branchId.Value != Guid.Empty)
                {
                    query = query.Where(s => s.IdLocationNavigation.IdWarehouseNavigation.IdBranch == branchId.Value);
                }

                var list = await query
                    .Include(s => s.IdProductNavigation)
                    .Include(s => s.IdLocationNavigation)
                        .ThenInclude(l => l.IdWarehouseNavigation)
                    .OrderBy(s => s.IdProductNavigation.Name)
                    .ToListAsync();

                return _mapper.Map<List<InventoryStockDTO>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar existencias de inventario.");
                throw;
            }
        }

        public async Task<bool> AdjustStockAsync(Guid idStock, decimal newQuantity, string? reason)
        {
            try
            {
                var stock = await _stockRepository.Query(s => s.IdStock == idStock)
                    .Include(s => s.IdProductNavigation)
                    .Include(s => s.IdLocationNavigation)
                    .FirstOrDefaultAsync();

                if (stock == null) throw new KeyNotFoundException("El registro de existencias no fue encontrado.");

                var oldQty = stock.Quantity;
                stock.Quantity = newQuantity;
                stock.LastUpdated = DateTime.UtcNow;

                var updated = await _stockRepository.Update(stock);
                if (updated)
                {
                    var msgDetails = $"Ajuste de stock para '{stock.IdProductNavigation?.Name}' en ubicación '{stock.IdLocationNavigation?.Code}': de {oldQty} a {newQuantity}. Motivo: {reason ?? "Ajuste manual"}";
                    await _actionLogService.LogActionAsync(null, "AJUSTE_INVENTARIO", "inventory_stocks", idStock.ToString(), msgDetails);
                }
                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ajustar existencias para el stock {Id}", idStock);
                throw;
            }
        }
    }
}
