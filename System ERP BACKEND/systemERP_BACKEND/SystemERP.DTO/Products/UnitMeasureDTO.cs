namespace SystemERP.DTO.Products;

public class UnitMeasureDTO
{
    public int IdUnitMeasure { get; set; }

    public string Description { get; set; } = null!;

    public string? Name { get; set; }

    public string? Type { get; set; }

    public bool? IsActive { get; set; }
}
