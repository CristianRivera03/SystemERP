using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class WarehouseCategory
{
    public int IdWarehouseCategory { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
