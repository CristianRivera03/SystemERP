using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Product
{
    public Guid IdProduct { get; set; }

    public int IdCategory { get; set; }

    public int IdProductType { get; set; }

    public int IdUnitMeasure { get; set; }

    public string Name { get; set; } = null!;

    public string? TaxCode { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? InternalCode { get; set; }

    public string? Barcode { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsTaxable { get; set; }

    public decimal? MinStock { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ProductType IdProductTypeNavigation { get; set; } = null!;

    public virtual UnitMeasure IdUnitMeasureNavigation { get; set; } = null!;

    public virtual ICollection<InventoryStock> InventoryStocks { get; set; } = new List<InventoryStock>();

    public virtual ICollection<ProductPresentation> ProductPresentations { get; set; } = new List<ProductPresentation>();
}
