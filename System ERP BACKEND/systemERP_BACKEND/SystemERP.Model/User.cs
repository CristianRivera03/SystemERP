using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class User
{
    public Guid IdUser { get; set; }

    public int IdRole { get; set; }

    public int IdCountry { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? DocumentId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? IdBranch { get; set; }

    public virtual ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();

    public virtual Branch? IdBranchNavigation { get; set; }

    public virtual Country IdCountryNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
