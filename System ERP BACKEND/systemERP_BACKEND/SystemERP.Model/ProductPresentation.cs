using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class ProductPresentation
{
    public Guid IdProductPresentation { get; set; }

    public Guid IdProduct { get; set; }

    public int IdPresentation { get; set; }

    public decimal Price { get; set; }

    public bool? IsActive { get; set; }

    public virtual Presentation IdPresentationNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
