using System;

namespace SystemERP.DTO.Entities;

public class CompanyDTO
{
    public Guid IdCompany { get; set; }
    public string BusinessName { get; set; } = null!;
    public string? TradeName { get; set; }
    public string TaxId { get; set; } = null!;
    public string? Nrc { get; set; }
    public string CommercialLine1 { get; set; } = null!;
    public string? CommercialLine2 { get; set; }
    public string? CommercialLine3 { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string DistrictId { get; set; } = null!;
    public string? DistrictName { get; set; }
    public string? AddressComplement { get; set; }
    public string? LogoUrl { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
}
