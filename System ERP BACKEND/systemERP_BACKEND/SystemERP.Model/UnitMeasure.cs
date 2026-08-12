using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class UnitMeasure
{
    public int IdUnitMeasure { get; set; }

    public string Description { get; set; } = null!;

    public string? Name { get; set; }

    public string? Type { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> ProductIdUnitMeasureNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductPurchaseUnits { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductSaleUnits { get; set; } = new List<Product>();
}
