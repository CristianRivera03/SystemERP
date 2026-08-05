namespace SystemERP.DTO.Products;

public class ProductDTO
{
    public Guid IdProduct { get; set; }

    public int IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public int IdProductType { get; set; }

    public string? ProductTypeDescription { get; set; }

    public int IdUnitMeasure { get; set; }

    public string? UnitMeasureDescription { get; set; }

    public string Name { get; set; } = null!;

    public string? TaxCode { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
