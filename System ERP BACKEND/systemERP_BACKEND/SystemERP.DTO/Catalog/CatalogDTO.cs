namespace SystemERP.DTO.Catalog;

public class CatalogDTO<TId>
{
    public TId Id { get; set; } = default!;

    public string Name { get; set; } = null!;
}

public class CatalogDTO : CatalogDTO<int>
{
}
