using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Warehouse
{
    public Guid IdWarehouse { get; set; }

    public Guid IdBranch { get; set; }

    public int IdWarehouseCategory { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual Branch IdBranchNavigation { get; set; } = null!;

    public virtual WarehouseCategory IdWarehouseCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
