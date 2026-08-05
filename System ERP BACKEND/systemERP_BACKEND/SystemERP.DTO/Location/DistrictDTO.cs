namespace SystemERP.DTO.Location;

public class DistrictDTO
{
    public string IdDistrict { get; set; } = null!;

    public string MunicipalityId { get; set; } = null!;

    public string? MunicipalityName { get; set; }

    public string Name { get; set; } = null!;
}
