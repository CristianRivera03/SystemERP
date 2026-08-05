using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Municipality
{
    public string IdMunicipality { get; set; } = null!;

    public string DepartmentId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
