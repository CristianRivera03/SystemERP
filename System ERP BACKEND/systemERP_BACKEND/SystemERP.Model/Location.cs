using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Location
{
    public Guid IdLocation { get; set; }

    public Guid IdWarehouse { get; set; }

    public string? Aisle { get; set; }

    public string? Rack { get; set; }

    public string? Level { get; set; }

    public string? Position { get; set; }

    public string? Code { get; set; }

    public int? Capacity { get; set; }

    public string? Notes { get; set; }

    public bool? IsActive { get; set; }

    public virtual Warehouse IdWarehouseNavigation { get; set; } = null!;

    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();
}
