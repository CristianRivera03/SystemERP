using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Inventory;

namespace SystemERP.BLL.Services.Contract
{
    public interface IInventoryService
    {
        Task<List<InventoryStockDTO>> GetStocksAsync(Guid? branchId = null, Guid? warehouseId = null);
        Task<bool> AdjustStockAsync(Guid idStock, decimal newQuantity, string? reason);
    }
}
