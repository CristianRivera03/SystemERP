using System;

namespace SystemERP.DTO.Inventory;

public class LocationDTO
{
    public Guid IdLocation { get; set; }
    public Guid IdWarehouse { get; set; }
    public string? WarehouseName { get; set; }
    public string? Aisle { get; set; }
    public string? Rack { get; set; }
    public string? Level { get; set; }
    public string? Position { get; set; }
    public string? Code { get; set; }
    public int? Capacity { get; set; }
    public string? Notes { get; set; }
    public bool? IsActive { get; set; }
}
