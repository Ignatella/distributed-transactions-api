using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models.Master;

public partial class Department
{
    public Guid DepartmentId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<MasterUser> MasterUsers { get; } = new List<MasterUser>();
}
