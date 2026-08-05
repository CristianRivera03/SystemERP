namespace SystemERP.DTO.Security;

public class ModuleDTO
{
    public int IdModule { get; set; }

    public string Name { get; set; } = null!;

    public string FrontendPath { get; set; } = null!;

    public string? Icon { get; set; }

    public bool? IsActive { get; set; }
}
