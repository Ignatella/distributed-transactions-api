using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models;

public partial class AccountType
{
    public Guid AccountTypeId { get; set; }

    public string Type { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
