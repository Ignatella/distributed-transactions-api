using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models;

public partial class Department
{
    public Guid DepartmentId { get; set; }

    public string Code { get; set; }

    public string DbCode { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
