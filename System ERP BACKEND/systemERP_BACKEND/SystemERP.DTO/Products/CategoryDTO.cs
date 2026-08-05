namespace SystemERP.DTO.Products;

public class CategoryDTO
{
    public int IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }
}
