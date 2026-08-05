using System.Collections.Generic;

namespace SystemERP.DTO.Security;

public class RoleWithModulesDTO
{
    public int IdRole { get; set; }
    public string RoleName { get; set; } = null!;
    public List<ModuleDTO> Modules { get; set; } = new();
}
