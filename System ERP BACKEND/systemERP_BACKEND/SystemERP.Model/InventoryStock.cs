using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class InventoryStock
{
    public Guid IdStock { get; set; }

    public Guid IdProduct { get; set; }

    public Guid IdLocation { get; set; }

    public decimal Quantity { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Location IdLocationNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
