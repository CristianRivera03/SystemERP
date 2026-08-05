using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class Department
{
    public string IdDepartment { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}
