using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Presentation
{
    public int IdPresentation { get; set; }

    public string Name { get; set; } = null!;

    public int UnitQuantity { get; set; }

    public virtual ICollection<ProductPresentation> ProductPresentations { get; set; } = new List<ProductPresentation>();
}
