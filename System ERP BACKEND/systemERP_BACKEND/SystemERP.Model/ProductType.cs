using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class ProductType
{
    public int IdProductType { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
