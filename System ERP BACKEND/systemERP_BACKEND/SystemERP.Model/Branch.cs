using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Branch
{
    public Guid IdBranch { get; set; }

    public Guid IdCompany { get; set; }

    public string Name { get; set; } = null!;

    public string DistrictId { get; set; } = null!;

    public string? AddressComplement { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual District District { get; set; } = null!;

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
