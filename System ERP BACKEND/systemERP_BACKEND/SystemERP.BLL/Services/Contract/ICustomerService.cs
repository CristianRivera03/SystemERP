using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Entities;

namespace SystemERP.BLL.Services.Contract
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetCustomersAsync(string? search = null);
        Task<CustomerDTO?> GetCustomerByIdAsync(Guid id);
        Task<CustomerDTO> CreateCustomerAsync(CustomerDTO dto);
        Task<CustomerDTO> UpdateCustomerAsync(CustomerDTO dto);
        Task<bool> ToggleStatusAsync(Guid id);
    }
}
