using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Supplier
{
    public Guid IdSupplier { get; set; }

    public string Name { get; set; } = null!;

    public string TaxId { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string DistrictId { get; set; } = null!;

    public string? AddressComplement { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Code { get; set; }

    public string? Website { get; set; }

    public virtual District District { get; set; } = null!;

    public virtual ICollection<SupplierContact> SupplierContacts { get; set; } = new List<SupplierContact>();
}
