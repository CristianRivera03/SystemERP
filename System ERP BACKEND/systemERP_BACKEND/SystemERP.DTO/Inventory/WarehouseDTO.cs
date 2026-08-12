using System;

namespace SystemERP.DTO.Inventory;

public class WarehouseDTO
{
    public Guid IdWarehouse { get; set; }
    public Guid IdBranch { get; set; }
    public string? BranchName { get; set; }
    public int IdWarehouseCategory { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}
