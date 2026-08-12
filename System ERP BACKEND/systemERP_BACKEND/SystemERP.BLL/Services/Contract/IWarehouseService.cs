using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Inventory;

namespace SystemERP.BLL.Services.Contract
{
    public interface IWarehouseService
    {
        Task<List<WarehouseCategoryDTO>> GetWarehouseCategoriesAsync();
        Task<List<WarehouseDTO>> GetAllWarehousesAsync();
        Task<WarehouseDTO?> GetWarehouseByIdAsync(Guid id);
        Task<WarehouseDTO> CreateWarehouseAsync(WarehouseDTO dto);
        Task<bool> UpdateWarehouseAsync(Guid id, WarehouseDTO dto);
        Task<bool> ToggleWarehouseStatusAsync(Guid id);
        Task<List<LocationDTO>> GetLocationsByWarehouseAsync(Guid warehouseId);
        Task<LocationDTO> CreateLocationAsync(LocationDTO dto);
    }
}
