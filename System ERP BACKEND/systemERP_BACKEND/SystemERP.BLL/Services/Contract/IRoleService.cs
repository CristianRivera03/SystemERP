using System.Collections.Generic;
using System.Threading.Tasks;
using SystemERP.DTO.Security;

namespace SystemERP.BLL.Services.Contract;

public interface IRoleService
{
    Task<List<RoleWithModulesDTO>> GetRolesWithModulesAsync();
    Task<List<ModuleDTO>> GetAllModulesAsync();
    Task<RoleWithModulesDTO> CreateRoleAsync(string roleName, List<int>? initialModuleIds = null);
    Task<bool> UpdateRolePermissionsAsync(UpdateRolePermissionsDTO dto);
    Task<bool> DeleteRoleAsync(int idRole);
}
