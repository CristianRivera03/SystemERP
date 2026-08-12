using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class SupplierContact
{
    public int IdSupplierContact { get; set; }

    public Guid IdSupplier { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    public virtual Supplier IdSupplierNavigation { get; set; } = null!;
}
