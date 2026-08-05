namespace SystemERP.DTO.Entities;

public class CustomerDTO
{
    public Guid IdCustomer { get; set; }

    public string Name { get; set; } = null!;

    public string? DocumentId { get; set; }

    public string? TaxId { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string DistrictId { get; set; } = null!;

    public string? DistrictName { get; set; }

    public string? MunicipalityName { get; set; }

    public string? DepartmentName { get; set; }

    public string? AddressComplement { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
