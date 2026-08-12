using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Entities;

namespace SystemERP.BLL.Services.Contract
{
    public interface IBranchService
    {
        Task<List<BranchDTO>> GetAllBranchesAsync();
        Task<BranchDTO?> GetBranchByIdAsync(Guid id);
        Task<BranchDTO> CreateBranchAsync(BranchDTO dto);
        Task<bool> UpdateBranchAsync(Guid id, BranchDTO dto);
        Task<bool> ToggleBranchStatusAsync(Guid id);
    }
}
