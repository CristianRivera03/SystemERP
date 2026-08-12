using System;
using System.Collections.Generic;

namespace SystemERP.Model;

public partial class District
{
    public string IdDistrict { get; set; } = null!;

    public string MunicipalityId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Municipality Municipality { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
