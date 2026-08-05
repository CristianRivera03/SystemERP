using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Country
{
    public int IdCountry { get; set; }

    public string CountryName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
