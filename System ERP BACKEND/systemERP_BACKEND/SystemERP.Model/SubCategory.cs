using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class SubCategory
{
    public int IdSubCategory { get; set; }

    public int IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
