using System;

namespace SystemERP.DTO.Security;

public class SessionDTO
{
    public Guid IdUser { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int IdRole { get; set; }
    public string? RoleName { get; set; }
    public int IdCountry { get; set; }
    public string? CountryName { get; set; }
    public string Token { get; set; } = null!;
    public List<ModuleDTO>? Modules { get; set; }
}
