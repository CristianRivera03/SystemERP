namespace SystemERP.DTO.Security;

public class RoleDTO
{
    public int IdRole { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
