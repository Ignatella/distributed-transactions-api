using System;
using System.Collections.Generic;

namespace DistributedTransactionsApi.Data.Models;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid FromUserId { get; set; }

    public Guid ToUserId { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User FromUser { get; set; }

    public virtual User ToUser { get; set; }
}
