namespace SystemERP.DTO.Products;

public class SubCategoryDTO
{
    public int IdSubCategory { get; set; }

    public int IdCategory { get; set; }

    public string? CategoryName { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }
}
