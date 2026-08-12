using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Entities;

namespace SystemERP.BLL.Services.Contract
{
    public interface ISupplierService
    {
        Task<List<SupplierDTO>> GetSuppliersAsync(string? search = null);
        Task<SupplierDTO?> GetSupplierByIdAsync(Guid id);
        Task<SupplierDTO> CreateSupplierAsync(SupplierDTO dto);
        Task<SupplierDTO> UpdateSupplierAsync(SupplierDTO dto);
        Task<bool> ToggleStatusAsync(Guid id);
        Task<SupplierContactDTO> AddContactAsync(SupplierContactDTO dto);
        Task<bool> DeleteContactAsync(int contactId);
    }
}
