namespace SystemERP.DTO.Security;

public class ActionLogDTO
{
    public Guid IdLog { get; set; }

    public Guid? IdUser { get; set; }

    public string? UserName { get; set; }

    public string Action { get; set; } = null!;

    public string AffectedTable { get; set; } = null!;

    public string RecordId { get; set; } = null!;

    public string? Details { get; set; }

    public string? SourceIp { get; set; }

    public DateTime? ActionDate { get; set; }
}
