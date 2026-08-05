namespace SystemERP.DTO.Products;

public class ProductPresentationDTO
{
    public Guid IdProductPresentation { get; set; }

    public Guid IdProduct { get; set; }

    public string? ProductName { get; set; }

    public int IdPresentation { get; set; }

    public string? PresentationName { get; set; }

    public decimal Price { get; set; }

    public bool? IsActive { get; set; }
}
