using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models.Master;

public partial class AccountType
{
    public Guid AccountTypeId { get; set; }

    public string Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
