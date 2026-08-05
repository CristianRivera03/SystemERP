using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class UnitMeasure
{
    public int IdUnitMeasure { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
