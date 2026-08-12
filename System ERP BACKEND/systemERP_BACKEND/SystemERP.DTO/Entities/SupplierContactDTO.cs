using System;

namespace SystemERP.DTO.Entities;

public class SupplierContactDTO
{
    public int IdSupplierContact { get; set; }

    public Guid IdSupplier { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }
}
