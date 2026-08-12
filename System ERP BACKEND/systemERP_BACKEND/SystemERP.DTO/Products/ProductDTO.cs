using System;

namespace SystemERP.DTO.Products;

public class ProductDTO
{
    public Guid IdProduct { get; set; }

    public int IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public int? IdSubCategory { get; set; }

    public string? SubCategoryName { get; set; }

    public int IdProductType { get; set; }

    public string? ProductTypeDescription { get; set; }

    public int IdUnitMeasure { get; set; }

    public string? UnitMeasureDescription { get; set; }

    public int? PurchaseUnitId { get; set; }

    public string? PurchaseUnitName { get; set; }

    public int? SaleUnitId { get; set; }

    public string? SaleUnitName { get; set; }

    public string Name { get; set; } = null!;

    public string? Sku { get; set; }

    public string? OriginalCode { get; set; }

    public string? InternalCode { get; set; }

    public string? Barcode { get; set; }

    public string? Size { get; set; }

    public string? Dimensions { get; set; }

    public string? Presentation { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsTaxable { get; set; }

    public decimal? MinStock { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
