using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models.Master;

public partial class MasterUser
{
    public Guid UserId { get; set; }

    public Guid DepartmentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual Department Department { get; set; }
}
