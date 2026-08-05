namespace SystemERP.DTO.Location;

public class MunicipalityDTO
{
    public string IdMunicipality { get; set; } = null!;

    public string DepartmentId { get; set; } = null!;

    public string? DepartmentName { get; set; }

    public string Name { get; set; } = null!;
}
