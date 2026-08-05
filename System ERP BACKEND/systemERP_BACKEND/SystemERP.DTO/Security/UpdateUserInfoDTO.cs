namespace SystemERP.DTO.Security;

public class UpdateUserInfoDTO
{
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? DocumentId { get; set; }
    public int IdCountry { get; set; }
}
