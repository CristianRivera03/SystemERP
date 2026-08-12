using System;

namespace SystemERP.DTO.Inventory;

public class InventoryStockDTO
{
    public Guid IdStock { get; set; }
    public Guid IdProduct { get; set; }
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public Guid IdLocation { get; set; }
    public string? LocationCode { get; set; }
    public string? WarehouseName { get; set; }
    public decimal Quantity { get; set; }
    public DateTime? LastUpdated { get; set; }
}
