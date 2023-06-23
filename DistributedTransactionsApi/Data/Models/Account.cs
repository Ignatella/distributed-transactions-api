using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models;

public partial class Account
{
    public Guid AccountId { get; set; }

    public Guid AccountTypeId { get; set; }

    public string AccountNumber { get; set; }

    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AccountType AccountType { get; set; }
}
