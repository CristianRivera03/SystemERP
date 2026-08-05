using System.Collections.Generic;

namespace SystemERP.DTO.Security;

public class UpdateRolePermissionsDTO
{
    public int IdRole { get; set; }
    public List<int> ModuleIds { get; set; } = new();
}
