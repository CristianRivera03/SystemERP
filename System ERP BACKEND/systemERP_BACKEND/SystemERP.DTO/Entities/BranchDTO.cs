using System;

namespace SystemERP.DTO.Entities;

public class BranchDTO
{
    public Guid IdBranch { get; set; }
    public Guid IdCompany { get; set; }
    public string? CompanyName { get; set; }
    public string Name { get; set; } = null!;
    public string DistrictId { get; set; } = null!;
    public string? DistrictName { get; set; }
    public string? MunicipalityId { get; set; }
    public string? MunicipalityName { get; set; }
    public string? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public string? AddressComplement { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
}
